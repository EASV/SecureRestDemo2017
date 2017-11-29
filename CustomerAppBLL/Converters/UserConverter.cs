using CustomerAppBLL.BusinessObjects;
using CustomerAppDAL.Entities;

namespace CustomerAppBLL.Converters
{
    class UserConverter
    {
        internal User Convert(UserBO user)
        {
			if (user == null) { return null; }
            return new User()
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Role = user.Role
            };
        }

        internal UserBO Convert(User user)
        {
			if (user == null) { return null; }
			return new UserBO()
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Role = user.Role
			};
        }
    }
}
