using GreenInspireLib.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenInspireLib.Services
{
    public class CategorySqlService
    {
        GreenInspireContext SqlContext;

        public CategorySqlService (GreenInspireContext sqlService)
        {
            this.SqlContext = sqlService;
        }

        public IEnumerable<Category> GetCategories(string? searchQuery = null)
        {
            List<Category> categories = new(SqlContext.Categories.AsNoTracking().ToList());
            if(searchQuery != null)
            {
                categories = categories.Where(c => c.CategoryName.ToLower().StartsWith(searchQuery.ToLower())).ToList();
            }
            return categories;
        }

        public Category? GetCategoryById(int categoryId)
        {
            if(categoryId <= 0)
            {
                throw new ArgumentException("There are no categories with 0 or smaller");
            }
            var category = SqlContext.Categories.AsNoTracking().FirstOrDefault(c => c.CategoryId == categoryId);
            return category;
        }

        public Category AddCategory(string categoryName)
        {
            if(CategoryExists(categoryName)) throw new ArgumentException("Category already exists");
            Category newCategory = new Category();
            newCategory.CategoryName = categoryName.First().ToString().ToUpper() + categoryName.Substring(1).ToLower();
            newCategory.Validate();
            SqlContext.Categories.Add(newCategory);
            SqlContext.SaveChanges();
            return newCategory;
        }

        public void DeleteCategory(int categoryId)
        {
            var category = SqlContext.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
            if (category == null )
            {
                throw new ArgumentException("There are no categories with the given id");
            }
            SqlContext.Categories.Remove(category);
            SqlContext.SaveChanges();
        }

        public Category UpdateCategory(Category newCategory)
        {
            newCategory.Validate();
            if (CategoryExists(newCategory.CategoryName))
            {
                throw new ArgumentException("Category already exists");
            }
            var categoryToUpdate = SqlContext.Categories.FirstOrDefault(c => c.CategoryId == newCategory.CategoryId);
            if (categoryToUpdate == null)
            {
                throw new ArgumentException("There are no categories with the given id");
            }
            categoryToUpdate.CategoryName = newCategory.CategoryName.First().ToString().ToUpper() + newCategory.CategoryName.Substring(1).ToLower();
            SqlContext.Update(categoryToUpdate);
            SqlContext.SaveChanges();
            return categoryToUpdate;
        }

        public bool CategoryExists(string categoryName)
        {
            return SqlContext.Categories.Any(c => c.CategoryName.ToLower() == categoryName.ToLower());
        }

        public int GetIdByCategoryName(string categoryName)
        {
            var category = SqlContext.Categories.FirstOrDefault(c => c.CategoryName.ToLower() == categoryName.ToLower());
            if (category == null) throw new ArgumentException("categorynavn findes ikke");
            return category.CategoryId;
        }



    }
}
