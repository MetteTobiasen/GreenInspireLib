using GreenInspireLib.Models;
using GreenInspireLib.Services;
using Microsoft.Data.SqlClient;
using GreenInspireLib.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenInspireLib.BusinessLogicLayer
{
    public class NewsfeedLogic
    {
        GreenInspireContext SqlContext;
        CategorySqlService CategorySqlService;
        NewsfeedSqlService NewsfeedSqlService;

        public NewsfeedLogic(GreenInspireContext sqlService, CategorySqlService categorySqlService, NewsfeedSqlService newsfeedSqlService)
        {
            this.SqlContext = sqlService;
            this.CategorySqlService = categorySqlService;
            this.NewsfeedSqlService = newsfeedSqlService;
        }

        

        public IEnumerable<NewsfeedWithCategoryDTO> GetAllNewsfeedsWithCategory(string? searchQuery = null)
        {
            List<NewsfeedWithCategoryDTO> newsfeedsWithCategory = new List<NewsfeedWithCategoryDTO>();
            var newsfeeds = NewsfeedSqlService.GetNewsfeeds(searchQuery);

            foreach (var news in newsfeeds)
            {
                //var category = news.Categories.FirstOrDefault();
                //if (category == null) throw new ArgumentException("category findes ikke");
                string category = "Energi og ressourcer";
                NewsfeedWithCategoryDTO objektToAdd = new NewsfeedWithCategoryDTO(news, news.CompanyUserId, category /*category.CategoryName*/);
                newsfeedsWithCategory.Add(objektToAdd);
            }
            return newsfeedsWithCategory;
        }

        public NewsfeedWithCategoryDTO GetNewsfeedWithCategoryById(int newsfeedId)
        {
            var newsfeed = NewsfeedSqlService.GetNewsfeedById(newsfeedId);
            if (newsfeed == null) throw new ArgumentException("Newsfeed don't exist with that id");
            var category = newsfeed.Categories.FirstOrDefault();
            if (category == null) throw new ArgumentException("can't find any category");
            NewsfeedWithCategoryDTO newsfeedWithCategory = new NewsfeedWithCategoryDTO(newsfeed, newsfeed.CompanyUserId, category.CategoryName);
            return newsfeedWithCategory;

        }

        public Newsfeed AddNewsfeedWithTransaction(string categoryName, Newsfeed newsfeed, string newsfeedImageFile, int userId)
        {
            Newsfeed newNewsfeed = new Newsfeed();
            using (var transaction = SqlContext.Database.BeginTransaction())
            {
                try
                {
                    newNewsfeed = NewsfeedSqlService.AddNewsfeed(newsfeed, newsfeedImageFile, userId);
                    AddCategoryToNewsfeedCategoryList(categoryName, newsfeed.NewsfeedId);
                    CategorySqlService.AddCategory(categoryName);
                    AddNewsfeedToCategoryNewsfeedList(categoryName, newsfeed.NewsfeedId);

                    transaction.Commit();
                    return newNewsfeed;
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new ArgumentException($"The newsfeed has not been saved, an error occurred: {ex}");
                }
            }
        }

        public void UpdateNewsfeedWithTransaction(string? categoryName, Newsfeed newsfeed, int userId, string? newsfeedImageFile = null)
        {
            using (var transaction = SqlContext.Database.BeginTransaction())
            {
                try
                {
                    //Updates the newsfeed
                    NewsfeedSqlService.UpdateNewsfeed(newsfeed, newsfeedImageFile);
                    if (categoryName != null)
                    {
                        //Add the new category if not null and not exist already
                        CategorySqlService.AddCategory(categoryName);
                        //Add the newsfeed to the new category newsfeedList
                        AddNewsfeedToCategoryNewsfeedList(categoryName, newsfeed.NewsfeedId);
                        //Removes the newsfeed from the old category newsfeed list
                        RemoveNewsfeedFromCategoryNewsfeedList(categoryName, newsfeed.NewsfeedId);
                    }
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new ArgumentException($"The newsfeed has not been updated, an error occurred: {ex}");
                }
            }
        }

        public Category AddNewsfeedToCategoryNewsfeedList(string categoryName, int newsfeedId)
        {
            Newsfeed newsfeedToAddToCategoryNewsfeedList = new Newsfeed();
            newsfeedToAddToCategoryNewsfeedList = SqlContext.Newsfeeds.FirstOrDefault(n => n.NewsfeedId == newsfeedId)
                ?? throw new AggregateException("Newsfeed not found with that newsfeed id");
            Category categoryNewsfeedListToUpdate = new Category();
            categoryNewsfeedListToUpdate = SqlContext.Categories.FirstOrDefault(c => c.CategoryName.ToLower() == categoryName.ToLower())
                ?? throw new AggregateException("category with that categoryname not found");
            categoryNewsfeedListToUpdate.Newsfeeds.Add(newsfeedToAddToCategoryNewsfeedList);
            SqlContext.Update(categoryNewsfeedListToUpdate);
            SqlContext.SaveChanges();
            return categoryNewsfeedListToUpdate;
        }

        public Category RemoveNewsfeedFromCategoryNewsfeedList(string categoryName, int newsfeedId)
        {
            Category? category = SqlContext.Categories.FirstOrDefault(c => c.CategoryName == categoryName.ToLower())
                ?? throw new ArgumentException("Categoryname don't exsist");
            Newsfeed? newsfeed = SqlContext.Newsfeeds.FirstOrDefault(n => n.NewsfeedId == newsfeedId)
                ?? throw new ArgumentException("Newsfeed with that id don't exist");
            // if category and newsfeed not null
            category.Newsfeeds.Remove(newsfeed);
            SqlContext.Update(category);
            SqlContext.SaveChanges();
            return category;
        }

        public Newsfeed AddCategoryToNewsfeedCategoryList(string categoryName, int newsfeedId)
        {
            Newsfeed? newsfeed = SqlContext.Newsfeeds.FirstOrDefault(n => n.NewsfeedId == newsfeedId)
                ?? throw new ArgumentException("Newsfeed with that id don't exist");
            Category? category = SqlContext.Categories.FirstOrDefault(c => c.CategoryName == categoryName.ToLower())
                ?? throw new ArgumentException("Categoryname don't exsist");

            // Check if the category is already associated with the newsfeed
            if (newsfeed.Categories.Contains(category)) throw new ArgumentException("katagorien er allerede tilføjet");
            {
                newsfeed.Categories.Add(category);
                SqlContext.Update(newsfeed);
                SqlContext.SaveChanges();
                return newsfeed;
            }
        }
    }
}
