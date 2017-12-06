using CustomerAppBLL.BusinessObjects;
using CustomerAppDAL.Entities;
using System.Linq;

namespace CustomerAppBLL.Converters
{
    public class CustomerConverter: IConverter<Customer, CustomerBO>
    {
        public Customer Convert(CustomerBO cust)
        {
            if (cust == null) { return null; }
            return new Customer()
            {
                Id = cust.Id,
                Addresses = cust.AddressIds?.Select(aId => new CustomerAddress() {
                    AddressId = aId,
                    CustomerId = cust.Id
                }).ToList(),
                FirstName = cust.FirstName,
                LastName = cust.LastName
            };
        }

        public CustomerBO Convert(Customer cust)
        {
			if (cust == null) { return null; }
            return new CustomerBO()
            {
                Id = cust.Id,
                AddressIds = cust.Addresses?.Select(a => a.AddressId).ToList(),
                FirstName = cust.FirstName,
                LastName = cust.LastName 
            };
        }
    }
}
