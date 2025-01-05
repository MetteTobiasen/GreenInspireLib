using System;
using GreenInspireLib.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenInspireLib.DTO
{
    public class NewsfeedWithCategoryDTO
    {
        private int newsfeedId;
        private byte[] newsfeedImage;
        private string newsfeedTitle;
        private string newsfeedText;
        private DateTime newsfeedTimestamp;
        private int companyUserId;
        private string categoryName;
        public NewsfeedWithCategoryDTO(Newsfeed newsfeed, int companyUserId, string categoryName)
        {
            this.newsfeedId = newsfeed.NewsfeedId;  
            this.newsfeedImage = newsfeed.NewsfeedImage;
            this.newsfeedTitle = newsfeed.Title; 
            this.newsfeedText = newsfeed.NewsfeedText;
            this.newsfeedTimestamp = newsfeed.NewsfeedTimestamp;
            this.companyUserId = companyUserId;
            this.categoryName = categoryName;
        }

    }
}
