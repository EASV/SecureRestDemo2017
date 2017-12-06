using System;
using CustomerAppBLL;
using CustomerAppBLL.BusinessObjects;
using CustomerAppBLL.Converters;
using CustomerAppBLL.Services;
using CustomerAppDAL;
using CustomerAppDAL.Entities;
using Moq;
using NUnit.Framework;

namespace CustomerAppBLLTest.Services
{
    [TestFixture]
    public class CustomerServiceTest
    {
        //Mock To isolate Test Object
        Mock<IDALFacade> dalFacade;
        Mock<IUnitOfWork> uow;
        Mock<ICustomerRepository> custRepo;
        Mock<IConverter<Customer, CustomerBO>> cConv;
        Mock<IConverter<Address, AddressBO>> aConv;

        //Entity to work with
        Customer custNoId = new Customer() { FirstName = "Kurt", LastName = "Olsen" };
        Customer custWithId = new Customer() { Id = 1, FirstName = "Kurt", LastName = "Olsen" };
        //BusinessObjects to work with
        CustomerBO custBONoId = new CustomerBO() { FirstName = "Kurt", LastName = "Olsen" };
        CustomerBO custBOWithId = new CustomerBO() { Id = 1, FirstName = "Kurt", LastName = "Olsen" };

        //Actual Test Object
        ICustomerService _service;

        [SetUp]
        public void Setup()
        {
            dalFacade = new Mock<IDALFacade>();
            uow = new Mock<IUnitOfWork>();
            custRepo = new Mock<ICustomerRepository>();

            cConv = new Mock<IConverter<Customer, CustomerBO>>();
            aConv = new Mock<IConverter<Address, AddressBO>>();
        }

        [TearDown]
        public void TearDown()
        {
            //Not used ATM, but could clean up if needed!
        }

        [Test]
        public void TestCreateCustomerService(){
            //Arange - Given -------------------------------- -------------------------------- --------------------------------

            //Repo Create method will return mocked custAft
            custRepo.Setup(repo => repo.Create(It.IsAny<Customer>())).Returns(custWithId);
            //UOW complete method will return 1
            uow.Setup(u => u.Complete()).Returns(1);
            //UOW GET CustomerRepo will return mocked custRepo
            uow.Setup(u => u.CustomerRepository).Returns(custRepo.Object);
            //cConv Will convert any CustomerBO to mocked custBef
            cConv.Setup(c => c.Convert(It.IsAny<CustomerBO>())).Returns(custNoId);
            //cConv Will convert any Customer to mocked custBOAft
            cConv.Setup(c => c.Convert(It.IsAny<Customer>())).Returns(custBOWithId);

            //dalFacade Will return mocked UOW
            dalFacade.Setup(dal => dal.UnitOfWork).Returns(uow.Object);

            //Create Service to TEST
            _service = new CustomerService(dalFacade.Object, cConv.Object);

            //Act - When -------------------------------- -------------------------------- --------------------------------
            var custCreated = _service.Create(custBONoId);

            //Assert - Then -------------------------------- -------------------------------- --------------------------------
            Assert.AreEqual(custBOWithId.Id, custCreated.Id);

            //Verify amount of calls!
            dalFacade.Verify(f => f.UnitOfWork, Times.Once());
            uow.Verify(u => u.CustomerRepository, Times.Once());
            cConv.Verify(c => c.Convert(
                It.Is<CustomerBO>(cbo =>
                                  cbo.Id == 0 && cbo.FirstName == "Kurt" &&
                                  cbo.LastName == "Olsen")), Times.Once());
            custRepo.Verify(r => r.Create(
                It.Is<Customer>(c => 
                                c.Id == 0 && c.FirstName == "Kurt" && 
                                c.LastName == "Olsen")), Times.Exactly(1));
            uow.Verify(u => u.Complete(), Times.Once());
            cConv.Verify(c => c.Convert(
                It.Is<Customer>(cEnt => 
                                cEnt.Id == 1 && cEnt.FirstName == "Kurt" &&
                                cEnt.LastName == "Olsen")), Times.Once());

        }
    }
}
