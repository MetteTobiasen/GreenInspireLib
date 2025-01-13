using Microsoft.VisualStudio.TestTools.UnitTesting;
using GreenInspireLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenInspireLib;
using GreenInspireLib.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using GreenInspireAPI;
using Moq;


namespace GreenInspireLib.Services.Tests
{
    [TestClass()]
    public class CategorySqlServiceTests
    {
        public static GreenInspireContext _dbContext;
        public static CategorySqlService _categorySqlService;

        //[ClassInitialize]
        //public static void InitOnce(TestContext context)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<GreenInspireContext>();
        //    var mockContext = new Mock<GreenInspireContext>(optionsBuilder.Options);
        //    _dbContext = mockContext.Object;
        //    _categorySqlService = new CategorySqlService(_dbContext);
        //}

        [ClassInitialize]
        public static void InitOnce(TestContext context)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GreenInspireContext>();
            var mockSet = new Mock<DbSet<Category>>();

            // Setup mock behavior for DbSet<Category> if needed
            // e.g., mockSet.Setup(m => m.FindAsync(It.IsAny<int>())).ReturnsAsync(new Category { CategoryId = 1, CategoryName = "Test" });

            var mockContext = new Mock<GreenInspireContext>(optionsBuilder.Options);
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object); // Assuming Categories is a DbSet<Category>

            _dbContext = mockContext.Object;
            _categorySqlService = new CategorySqlService(_dbContext);
        }

        [TestMethod()]
        public void GetCategoriesTest()
        {
            var categories = _categorySqlService.GetCategories();
            Assert.IsNotNull(categories);
            Assert.IsInstanceOfType(categories, typeof(List<Category>));
            Assert.AreEqual(13, categories.Count());
        }

        [TestMethod()]
        public void GetCategoryByIdTest()
        {
            var category = _categorySqlService.GetCategoryById(1);
            Assert.IsNotNull(category);
            Assert.AreEqual(1, category.CategoryId);
        }

        [TestMethod()]
        public void AddCategoryTest()
        {
            string newCategoryName = "New Category";
            _categorySqlService.AddCategory(newCategoryName);
            var addedCategory = _categorySqlService.GetCategories("New Category").FirstOrDefault();
            //var addedCategory = _categorySqlService.GetCategoryById(newCategory.CategoryId);
            Assert.IsNotNull(addedCategory);
            Assert.AreEqual("New Category", addedCategory.CategoryName);
        }

        [TestMethod()]
        public void DeleteCategoryTest()
        {
            var category = new Category { CategoryId = 1, CategoryName = "New Category" };
            _categorySqlService.AddCategory(category.CategoryName);
            _categorySqlService.DeleteCategory(category.CategoryId);
            var deletedCategory = _categorySqlService.GetCategoryById(category.CategoryId);
            Assert.IsNull(deletedCategory);
        }

        [TestMethod()]
        public void UpdateCategoryTest()
        {
            var category = new Category { CategoryId = 1, CategoryName = "Old Name" };
            _categorySqlService.AddCategory(category.CategoryName);
            category.CategoryName = "Updated Name";
            _categorySqlService.UpdateCategory(category);
            var updatedCategory = _categorySqlService.GetCategoryById(category.CategoryId);
            Assert.AreEqual("Updated Name", updatedCategory.CategoryName);
        }

        [TestMethod()]
        public void CategoryExistsTest()
        {
            var category = new Category { CategoryName = "new Category" };
            _categorySqlService.AddCategory(category.CategoryName);
            var exists = _categorySqlService.CategoryExists(category.CategoryName);
            Assert.IsTrue(exists);
        }
    }
}