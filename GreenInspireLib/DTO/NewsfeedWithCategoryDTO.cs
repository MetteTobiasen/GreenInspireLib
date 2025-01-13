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

        public int newsfeedId { get; private set; }
        public byte[] newsfeedImage { get; private set; }
        public string newsfeedTitle { get; private set; }
        public string newsfeedText { get; private set; }
        public DateTime newsfeedTimestamp { get; private set; }
        public int companyUserId { get; private set; }
        public string categoryName { get; private set; }

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
