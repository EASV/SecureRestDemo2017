using System;
using System.Collections.Generic;
using System.Text;
using CustomerAppDAL;
using System.Linq;
using CustomerAppDAL.Entities;
using CustomerAppBLL.BusinessObjects;
using CustomerAppBLL.Converters;

namespace CustomerAppBLL.Services
{
    public class CustomerService : ICustomerService
    {
        IConverter<Customer, CustomerBO> _cConv;
        IConverter<Address, AddressBO> _aConv;
        IDALFacade _facade;
        public CustomerService(IDALFacade facade,
                           IConverter<Customer, CustomerBO> cConv = null,
                           IConverter<Address, AddressBO> aConv = null)
        {
            
            _facade = facade;
            _cConv = cConv ?? new CustomerConverter();
            _aConv = aConv ?? new AddressConverter();
        }
        
        public CustomerBO Create(CustomerBO cust)
        {
            using(var uow = _facade.UnitOfWork)
            {
                var newCust = uow.CustomerRepository.Create(_cConv.Convert(cust));
				uow.Complete();
                return _cConv.Convert(newCust);
            }
        }

        public void CreateAll(List<CustomerBO> customers)
        {
            using (var uow = _facade.UnitOfWork)
            {
                foreach (var customer in customers)
                {
                    uow.CustomerRepository.Create(_cConv.Convert(customer));
                    
                }
                uow.Complete();
            }
        }

        public CustomerBO Delete(int Id)
        {
			using (var uow = _facade.UnitOfWork)
			{
				var newCust = uow.CustomerRepository.Delete(Id);
				uow.Complete();
                return _cConv.Convert(newCust);
			}
        }

        public CustomerBO Get(int Id)
        {
            using (var uow = _facade.UnitOfWork)
			{
                //1. Get and convert the customer
                var cust = _cConv.Convert(uow.CustomerRepository.Get(Id));

                //2. Get All related Addresses from AddressRepository using addressIds
                //3. Convert and Add the Addresses to the CustomerBO

                /*cust.Addresses = cust.AddressIds?
                    .Select(id => aConv.Convert(uow.AddressRepository.Get(id)))
                    .ToList();*/

                cust.Addresses = uow.AddressRepository.GetAllById(cust.AddressIds)
                    .Select(a => _aConv.Convert(a))
                    .ToList();

                //4. Return the Customer
                return cust;
            }
        }

        public List<CustomerBO> GetAll()
        {
			using (var uow = _facade.UnitOfWork)
			{
                //Customer -> CustomerBO
                //return uow.CustomerRepository.GetAll();
                return uow.CustomerRepository.GetAll().Select(_cConv.Convert).ToList();
			}
        }

        public List<CustomerBO> 
        GetAllByFirstName(string t, int ps, int cp)
        {
            using (var uow = _facade.UnitOfWork){
                var skip = (ps * cp) - ps;
                return uow.CustomerRepository
                          .GetAll()
                          .Where(c => c.FirstName.Contains(t))
                          .Skip(skip)
                          .Take(ps)
                          .Select(c => _cConv.Convert(c))
                          .ToList();
            }
        }

        public CustomerBO Update(CustomerBO cust)
        {
            using (var uow = _facade.UnitOfWork)
            {
                var customerFromDb = uow.CustomerRepository.Get(cust.Id);
				if (customerFromDb == null)
				{
					throw new InvalidOperationException("Customer not found");
				}

                var customerUpdated = _cConv.Convert(cust);
				customerFromDb.FirstName = customerUpdated.FirstName;
				customerFromDb.LastName = customerUpdated.LastName;

                //1. Remove All, except the "old" ids we 
                //      wanna keep (Avoid attached issues)
                customerFromDb.Addresses.RemoveAll(
                    ca => !customerUpdated.Addresses.Exists(
                        a => a.AddressId == ca.AddressId &&
                        a.CustomerId == ca.CustomerId));

                //2. Remove All ids already in database 
                //      from customerUpdated
                customerUpdated.Addresses.RemoveAll(
                    ca => customerFromDb.Addresses.Exists(
                        a => a.AddressId == ca.AddressId &&
                        a.CustomerId == ca.CustomerId));

                //3. Add All new CustomerAddresses not 
                //      yet seen in the DB
                customerFromDb.Addresses.AddRange(
                    customerUpdated.Addresses);

                uow.Complete();
                return _cConv.Convert(customerFromDb);
            }

        }

    }
}
