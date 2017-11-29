using CustomerAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAppDAL
{
    public interface IUserRepository
    {
        //C
        User Create(User user);
        //R
        IEnumerable<User> GetAll();
        IEnumerable<User> GetAllById(List<int> ids);
        User Get(int Id);
        //U
        //No Update for Repository, It will be the task of Unit of Work
        //D
        User Delete(int Id);
    }
}
