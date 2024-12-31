using GreenInspireLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static System.Net.Mime.MediaTypeNames;
//using System.Drawing;
using SixLabors.ImageSharp;




namespace GreenInspireLib.Services
{
    public class NewsfeedSqlService
    {
        GreenInspireContext SqlContext;

        public NewsfeedSqlService(GreenInspireContext sqlService)
        {
            this.SqlContext = sqlService;
        }

        public IEnumerable<Newsfeed> GetNewsfeeds(string? searchQuery = null)
        {
            List<Newsfeed> newsfeeds = new(SqlContext.Newsfeeds.AsNoTracking().ToList());
            if (searchQuery != null)
            {
                newsfeeds = newsfeeds.Where(n => n.Title.ToLower().Contains(searchQuery.ToLower())).ToList();
                newsfeeds = newsfeeds.Where(n => n.Categories.Any(c => c.CategoryName.ToLower().Contains(searchQuery.ToLower()))).ToList();
            }
            return newsfeeds;
        }

        public Newsfeed? GetNewsfeedById(int newsfeedId)
        {
            if (newsfeedId <= 0)
            {
                throw new ArgumentException("There are no newsfeeds with 0 or smaller");
            }
            var newsfeed = SqlContext.Newsfeeds.AsNoTracking().FirstOrDefault(n => n.NewsfeedId == newsfeedId);
            return newsfeed;
        }

        public Newsfeed AddNewsfeed(Newsfeed newsfeed, string imageFile, int companyUserId)
        {
            newsfeed.Validate();
            newsfeed.Title = newsfeed.Title.First().ToString().ToUpper() + newsfeed.Title.Substring(1).ToLower();
            newsfeed.NewsfeedText = newsfeed.NewsfeedText.First().ToString().ToUpper() + newsfeed.NewsfeedText.Substring(1);
            newsfeed.NewsfeedTimestamp = DateTime.Now;
            newsfeed.NewsfeedImage = ConvertImageToByte(imageFile);
            newsfeed.CompanyUserId = companyUserId; 
            SqlContext.Newsfeeds.Add(newsfeed);
            SqlContext.SaveChanges();
            return newsfeed;
        }

        public void DeleteNewsfeed(int newsfeedId)
        {
            var newsfeed = SqlContext.Newsfeeds.FirstOrDefault(n => n.NewsfeedId == newsfeedId);
            if (newsfeed == null)
            {
                throw new ArgumentException("There are no newsfeeds with the given id");
            }
            SqlContext.Newsfeeds.Remove(newsfeed);
            SqlContext.SaveChanges();
        }

        public void DeleteNewsfeedByUser(int newsfeedId, int UserId)
        {
            var newsfeed = SqlContext.Newsfeeds.FirstOrDefault(n => n.NewsfeedId == newsfeedId && n.CompanyUserId == UserId);
            if (newsfeed == null)
            {
                throw new ArgumentException("There are no newsfeeds with the given id");
            }
            SqlContext.Newsfeeds.Remove(newsfeed);
            SqlContext.SaveChanges();
        }

        public IEnumerable<Newsfeed> GetNewsfeedsByUser(int userId)
        {
            List<Newsfeed> newsfeeds = new(SqlContext.Newsfeeds.AsNoTracking().Where(n => n.CompanyUserId == userId).ToList());
            return newsfeeds;
        }

        public Newsfeed UpdateNewsfeed(Newsfeed newNewsfeed, string? imagePath)
        {
            newNewsfeed.Validate();
            var newsfeedToUpdate = SqlContext.Newsfeeds.FirstOrDefault(n => n.NewsfeedId == newNewsfeed.NewsfeedId) 
                ?? throw new ArgumentException("There are no newsfeeds with the given id");   
            //Update newsfeed if not null and validate ok
            newsfeedToUpdate.Title = newNewsfeed.Title.First().ToString().ToUpper() + newNewsfeed.Title.Substring(1).ToLower();
            newsfeedToUpdate.NewsfeedText = newNewsfeed.NewsfeedText.First().ToString().ToUpper() + newNewsfeed.NewsfeedText.Substring(1);            
            if(imagePath != null) newsfeedToUpdate.NewsfeedImage = ConvertImageToByte(imagePath);                    
            newsfeedToUpdate.NewsfeedTimestamp = DateTime.Now;
            SqlContext.Update(newsfeedToUpdate);
            SqlContext.SaveChanges(); 
            return newsfeedToUpdate;
        }

        public Byte[] ConvertImageToByte(string imagePath) 
        {
            //if (string.IsNullOrWhiteSpace(imagePath)) throw new ArgumentException("Image path cannot be null or empty.", nameof(imagePath));           
            try
            {
                Byte[] bytes = System.IO.File.ReadAllBytes(imagePath);

                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException("The specified image file was not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading the image file.", ex);
            }
        }

        //public void ValidateImage(string filePath, string? outputPath = null) // Added return type 'void' and made outputPath nullable
        //{
        //    int maxWidth = 1920;
        //    int maxHeight = 1080;

        //    try
        //    {
        //        // Check if file exists
        //        if (!System.IO.File.Exists(filePath))
        //        {
        //            Console.WriteLine("File does not exist.");
        //            return; // Exit if file does not exist
        //        }

        //        using (var tempStream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(filePath)))
        //        {
        //            using (Image image = image.Load(tempStream)) 
        //            {
        //                int width = image.Width;
        //                int height = image.Height;

        //                // Check if the resolution is valid
        //                if (!(width <= maxWidth && height <= maxHeight))
        //                {
        //                    Console.WriteLine($"Image size is not valid, maximum size is: {maxWidth}x{maxHeight} pixels.");
        //                }

        //                Console.WriteLine($"Image resolution is too large: {width}x{height} pixels. Resizing to fit {maxWidth}x{maxHeight} pixels.");
        //            } 
        //        }
        //        // Load the image
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }
        //}
    }
}

