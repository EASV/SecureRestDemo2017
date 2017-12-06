using System;
using System.Collections.Generic;
using System.Text;
using CustomerAppBLL.BusinessObjects;
using CustomerAppDAL;
using CustomerAppBLL.Converters;
using System.Linq;
using CustomerAppDAL.Entities;

namespace CustomerAppBLL.Services
{
    class UserService : IUserService
    {
        IConverter<User, UserBO> _conv;
        private IDALFacade _facade;
        public UserService(IDALFacade facade,
                           IConverter<User, UserBO> conv = null)
        {
            _facade = facade;
            _conv = conv ?? new UserConverter();
        }

        public UserBO Create(UserBO user)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var userEntity = uow.UserRepository.Create(_conv.Convert(user));
                uow.Complete();
                return _conv.Convert(userEntity);
            }
        }

        public UserBO Delete(int Id)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var userEntity = uow.UserRepository.Delete(Id);
                uow.Complete();
                return _conv.Convert(userEntity);
            }
        }

        public UserBO Get(int Id)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var userEntity = uow.UserRepository.Get(Id);
                return _conv.Convert(userEntity);
            }
        }

        public List<UserBO> GetAll()
        {
            using (var uow = _facade.UnitOfWork)
            {
                return uow.UserRepository.GetAll().Select(_conv.Convert).ToList();
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
                return _conv.Convert(userEntity);
            }
        }
    }
}
