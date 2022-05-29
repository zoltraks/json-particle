using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

#pragma warning disable 0649

namespace BigBytes.JsonParticle.Test
{
    [TestClass]
    public class MapperTest
    {
        internal class Mock
        {
            public class Person : Json<Person>
            {
                public string Name;
                public string City;
                public int Age;
            }

            public class Customer : Json<Customer>
            {
                public string Name;
                public string City;
                public string Office;
            }
        }

        [TestMethod]
        public void Map()
        {
            Mock.Person person;
            Mock.Customer customer;
            
            person = new Mock.Person()
            {
                Name = "Andy",
                Age = 21,
                City = "Warsaw",
            };

            customer = new Mapper<Mock.Customer>()
                .Map(person);

            Assert.IsNotNull(customer);
            Assert.AreEqual("Andy", customer.Name);
            Assert.AreEqual("Warsaw", customer.City);
            Assert.IsNull(customer.Office);

            Debug.WriteLine(customer.Name);              // Outputs: "Andy"
            Debug.WriteLine(customer.City);              // Outputs: "Warsaw"
            Debug.WriteLine(customer.Office ?? "NULL");  // Outputs: "NULL"

            person = new Mapper<Mock.Person>()
                .Map(customer);

            Debug.WriteLine(person.Name);  // Outputs: "Andy"
            Debug.WriteLine(person.City);  // Outputs: "Warsaw"
            Debug.WriteLine(person.Age);   // Outputs: "0"
        }
    }
}
