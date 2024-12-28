using GreenInspireLib.Models;
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

        public Category AddCategory(Category category)
        {
            category.Validate();
            if(CategoryExists(category.CategoryName))
            {
                throw new ArgumentException("Category already exists");
            }
            category.CategoryName = category.CategoryName.First().ToString().ToUpper() + category.CategoryName.Substring(1).ToLower();
            SqlContext.Categories.Add(category);
            SqlContext.SaveChanges();
            return category;
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



    }
}
