using CustomerAppBLL.Services;
using CustomerAppDAL;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using CustomerAppBLL.Converters;

namespace CustomerAppBLL
{
    public class BLLFacade : IBLLFacade
    {
        private DALFacade facade;

        public BLLFacade(IConfiguration conf){
            facade = new DALFacade(new DbOptions()
            {
                ConnectionString = conf.GetConnectionString("DefaultConnection"),
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            });
        }

        public ICustomerService CustomerService => new CustomerService(facade); 
        public IOrderService OrderService => new OrderService(facade);
        public IAddressService AddressService => new AddressService(facade);
        public IUserService UserService => new UserService(facade);

    }
}
