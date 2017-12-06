using System;
using System.Text.RegularExpressions;
using CustomerAppDAL.Entities;

namespace CustomerAppDAL.Helpers
{
    public class UserValidator
    {
        User _user;
        public UserValidator(User user)
        {
            if(user == null)
            {
                throw new ArgumentException("User cannot be null");
            } 
            if(user.Username == null)
            {
                throw new ArgumentException("Username cannot be null");
            }
            _user = user;
        }

        public void ValidateUserName()
        {
            if(_user.Username.Length < 6){
                throw new ArgumentException("Username to Short");
            }
            if (_user.Username.Length > 20)
            {
                throw new ArgumentException("Username to Long");
            }
            if (Regex.IsMatch(_user.Username, @"(.)\1\1"))
            {
                throw new ArgumentException("Username Can not contain same char 3 or more times in a row");
            }
            if(_user.Username.Contains(",") ||
               _user.Username.Contains(".") ||
               _user.Username.Contains("(") ||
               _user.Username.Contains(")") ||
               _user.Username.Contains("/") ||
               _user.Username.Contains("_"))
            {
                throw new ArgumentException("Username Cannot contain Special Chars like '.', ',', '/', '_', '(' and ')'");
            }
        }
    }
}
