using System;
using CustomerAppDAL.Context;
using CustomerAppDAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerAppDAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private CustomerAppContext context;

        public UnitOfWork(DbOptions opt)
        {
            DbContextOptions<CustomerAppContext> options;
            if (opt.Environment == "Development" && String.IsNullOrEmpty(opt.ConnectionString))
            {
                options = new DbContextOptionsBuilder<CustomerAppContext>()
                   .UseInMemoryDatabase("TheDB")
                   .Options;
            }
            else
            {
                options = new DbContextOptionsBuilder<CustomerAppContext>()
                .UseSqlServer(opt.ConnectionString)
                    .Options;
            }

            context = new CustomerAppContext(options);
        }

        //Fat Arrow!
        public ICustomerRepository CustomerRepository { get => new CustomerRepository(context); }
        public IOrderRepository OrderRepository { get => new OrderRepository(context); }

        //Fat Arrow Body!
        public IAddressRepository AddressRepository => new AddressRepository(context);

        //Without Fat Arrow!
        public IUserRepository UserRepository { get { return new UserRepository(context); } }

        public int Complete()
        {
            //The number of objects written to the underlying database.
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
