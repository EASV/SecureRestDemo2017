using System;
using CustomerAppBLL.BusinessObjects;
using CustomerAppBLL.Converters;
using CustomerAppDAL.Entities;
using NUnit.Framework;

namespace CustomerAppBLLTest.Converters
{
    [TestFixture]
    public class CustomerConverterTest
    {
        IConverter<Customer, CustomerBO> converter;
            
        [SetUp]
        public void Setup(){
            converter = new CustomerConverter();
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void ConvertBOToEntNull (){
            Customer ent = converter.Convert((CustomerBO)null);

            Assert.IsNull(ent);
        }

        [Test]
        public void ConvertBOToEntId()
        {
            var cust = new CustomerBO() { Id = 1 };
            Customer bo = converter.Convert(cust);

            Assert.AreEqual(bo.Id, 1);
        }

    }
}
