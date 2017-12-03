using System;
using CustomerAppDAL;
using Moq;
using NUnit.Framework;

namespace CustomerAppBLLTest.Services
{
    [TestFixture]
    public class CustomerServiceTest
    {
        public CustomerServiceTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Mock<DALFacade> dalFacade = new Mock<DALFacade>();
            Mock<ICustomerRepository> geographicsRepository = new Mock<ICustomerRepository>();

            GeographicService geoService = new GeographicService(geographicsRepository.Object);

            // Setup Mock
            geographicsRepository
                .Setup(x => x.SaveCountry(It.IsAny<Country>()))
                .Returns(1);

            var id = geoService.AddCountry("Jamaica");

        }

        [TearDown]
        public void TearDown()
        {

        }


    }
}
