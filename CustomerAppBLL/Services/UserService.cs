using System;
using System.Collections.Generic;
using System.Text;
using CustomerAppBLL.BusinessObjects;
using CustomerAppDAL;
using CustomerAppBLL.Converters;
using System.Linq;

namespace CustomerAppBLL.Services
{
    class UserService : IUserService
    {
        UserConverter conv = new UserConverter();
        private DALFacade _facade;
        public UserService(DALFacade facade)
        {
            _facade = facade;
        }

        public UserBO Create(UserBO user)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var userEntity = uow.UserRepository.Create(conv.Convert(user));
                uow.Complete();
                return conv.Convert(userEntity);
            }
        }

        public UserBO Delete(int Id)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var userEntity = uow.UserRepository.Delete(Id);
                uow.Complete();
                return conv.Convert(userEntity);
            }
        }

        public UserBO Get(int Id)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var userEntity = uow.UserRepository.Get(Id);
                return conv.Convert(userEntity);
            }
        }

        public List<UserBO> GetAll()
        {
            using (var uow = _facade.UnitOfWork)
            {
                return uow.UserRepository.GetAll().Select(conv.Convert).ToList();
            }
        }

        public UserBO Update(UserBO user)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var userEntity = uow.UserRepository.Get(user.Id);
                if(userEntity == null)
                {
                    throw new InvalidOperationException("User not found");
                }
                uow.Complete();
                return conv.Convert(userEntity);
            }
        }
    }
}
