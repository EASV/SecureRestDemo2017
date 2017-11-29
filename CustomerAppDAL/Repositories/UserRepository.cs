using System;
using System.Collections.Generic;
using System.Linq;
using CustomerAppDAL.Context;
using CustomerAppDAL.Entities;

namespace CustomerAppDAL.Repositories
{
    class UserRepository : IUserRepository
    {
        CustomerAppContext _context;
        public UserRepository(CustomerAppContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            return user;
        }

        public User Delete(int Id)
        {
            var user = Get(Id);
            _context.Users.Remove(user);
            return user;
        }

        public User Get(int Id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == Id);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public IEnumerable<User> GetAllById(List<int> ids)
        {
            if (ids == null) { return null; }

            return _context.Users.Where(a => ids.Contains(a.Id));
        }
    }
}
