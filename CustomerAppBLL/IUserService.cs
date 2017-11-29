using CustomerAppBLL.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerAppBLL
{
    public interface IUserService
    {
        //C
        UserBO Create(UserBO user);
        //R
        List<UserBO> GetAll();
        UserBO Get(int Id);
        //U
        UserBO Update(UserBO user);
        //D
        UserBO Delete(int Id);
    }
}
