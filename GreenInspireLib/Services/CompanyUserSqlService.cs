using GreenInspireLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace GreenInspireLib.Services
{
    public class CompanyUserSqlService
    {
        GreenInspireContext SqlContext;

        public CompanyUserSqlService(GreenInspireContext sqlService)
        {
            this.SqlContext = sqlService;
        }
        
        public IEnumerable<CompanyUser> GetUsers(string? searchQuery = null)
        {
            List<CompanyUser> users = new(SqlContext.CompanyUsers.AsNoTracking().ToList());
            if (searchQuery != null)
            {
                users = users.Where(c => c.CompanyName.ToLower().Contains(searchQuery.ToLower())).ToList();
            }
            return users;

        }

        public CompanyUser? GetUserById(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("There are no user with id 0 or smaller");
            }
            var user = SqlContext.CompanyUsers.AsNoTracking().FirstOrDefault(c => c.CompanyUserId == userId);
            return user;
        }

        public CompanyUser AddUser(CompanyUser user, string logoFile)
        {
            user.Validate();
            if (UserExists(user.CompanyName))
            {
                throw new ArgumentException("User with that name already exists");
            }
            user.CompanyName = user.CompanyName.First().ToString().ToUpper() + user.CompanyName.Substring(1).ToLower();
            user.CompanyLogo = ConvertLogoToByte(logoFile);
            SqlContext.CompanyUsers.Add(user);
            SqlContext.SaveChanges();
            return user;
        }
        public CompanyUser UpdateUser(CompanyUser user, string? logoFilePath = null)
        {
            user.Validate();
            if (UserExists(user.CompanyName))
            {
                throw new ArgumentException("User with that username already exists, choose another username");
            }
            var userToUpdate = SqlContext.CompanyUsers.FirstOrDefault(n => n.CompanyUserId == user.CompanyUserId);
            if (userToUpdate == null) throw new ArgumentNullException("There are no user with the given id");
            
            userToUpdate.CompanyName = user.CompanyName.First().ToString().ToUpper() + user.CompanyName.Substring(1).ToLower();
            userToUpdate.CompanySize = user.CompanySize;
            userToUpdate.Email = user.Email;
            userToUpdate.UserPassword = user.UserPassword;
            userToUpdate.CompanyCvr = user.CompanyCvr;
            if (logoFilePath != null)
            {
                userToUpdate.CompanyLogo = ConvertLogoToByte(logoFilePath);
            }
            SqlContext.Update(userToUpdate);
            SqlContext.SaveChanges();
            return userToUpdate;
        }

        public bool UserExists(string userName)
        {
            return SqlContext.CompanyUsers.Any(c => c.CompanyName.ToLower() == userName.ToLower());
        }

        public Byte[] ConvertLogoToByte(string logoPath)
        {
            if (string.IsNullOrWhiteSpace(logoPath))
            {
                throw new ArgumentException("Logo path cannot be null or empty.", nameof(logoPath));
            }
            try
            {
                Byte[] bytes = System.IO.File.ReadAllBytes(logoPath);

                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException("The specified logo file was not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while reading the logo file.", ex);
            }
        }
    }   
}
