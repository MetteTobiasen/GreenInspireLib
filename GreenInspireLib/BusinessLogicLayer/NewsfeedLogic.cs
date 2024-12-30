using GreenInspireLib.Models;
using GreenInspireLib.Services;
using Microsoft.Data.SqlClient;
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
        
        struct Values
            {
                Category category;
                Newsfeed newsfeed;
            }

        public void /*IEnumerable<(Category, Newsfeed)>*/ AddNewsfeedWithTransaction(Category category, Newsfeed newsfeed, string newsfeedImageFile, int userId)
        {
            //List<(Category, Newsfeed)> values = new List<(Category, Newsfeed)>();
            using (var transaction = SqlContext.Database.BeginTransaction())
            {
                try
                {
                    NewsfeedSqlService.AddNewsfeed(newsfeed, newsfeedImageFile, userId);
                    CategorySqlService.AddCategory(category);
                    transaction.Commit();
                    //values.AddRange(category, newsfeed);
                    //return values;
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
            using(var  transaction = SqlContext.Database.BeginTransaction())
            {
                try
                {
                    var newCategory = new Category();
                    var oldCategory = SqlContext.Categories.FirstOrDefault(c => c.CategoryId == userId);
                    if ((categoryName != null) && !CategorySqlService.CategoryExists(categoryName) && oldCategory != null)
                    {                       
                        newCategory.CategoryName = categoryName;
                        newCategory.Newsfeeds.Add(newsfeed);
                        newCategory.Validate();
                        CategorySqlService.AddCategory(newCategory);
                        oldCategory.Newsfeeds.Remove(newsfeed);
                    }
                    //if (newsfeedImageFile != null)
                    //{
                    //    Byte[] newImage = NewsfeedSqlService.ConvertImageToByte(newsfeedImageFile);
                    //    newsfeed.NewsfeedImage = newImage;
                    //}
                    newsfeed.NewsfeedTimestamp = DateTime.Now;
                    newsfeed.Categories.Add(newCategory);
                    Newsfeed updatedNewsfeed = NewsfeedSqlService.UpdateNewsfeed(newsfeed, newsfeedImageFile);
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new ArgumentException($"The newsfeed has not been updated, an error occurred: {ex}");
                }
                
            
            
            }
            
        }
    }
}
