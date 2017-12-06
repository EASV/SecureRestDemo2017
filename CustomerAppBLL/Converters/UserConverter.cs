using CustomerAppBLL.BusinessObjects;
using CustomerAppDAL.Entities;

namespace CustomerAppBLL.Converters
{
    public class UserConverter: IConverter<User, UserBO>
    {
        public User Convert(UserBO user)
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

        public UserBO Convert(User user)
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
