using GreenInspireLib.Models;
using GreenInspireLib.Services;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
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

        public void Add(Category category, Newsfeed newsfeed)
        {

        }     
    }
}
