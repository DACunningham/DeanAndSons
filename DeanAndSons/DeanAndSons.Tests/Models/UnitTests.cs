using DeanAndSons.Models;
using DeanAndSons.Models.IMS.ViewModels;
using DeanAndSons.Models.WAP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeanAndSons.Tests.Models
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void IsCustomerTest19()
        {
            // Arrange
            var r1 = new ApplicationUser();
            var r2 = new Customer();
            var r3 = new Staff();

            // Act
            var rb1 = ApplicationUser.IsCustomer(r1);
            var rb2 = ApplicationUser.IsCustomer(r2);
            var rb3 = ApplicationUser.IsCustomer(r3);
            var rb4 = ApplicationUser.IsCustomer(null);


            // Assert
            Assert.AreEqual<bool>(false, rb1);
            Assert.AreEqual<bool>(true, rb2);
            Assert.AreEqual<bool>(false, rb3);
            Assert.AreEqual<bool>(false, rb4);
        }

        [TestMethod]
        public void addContactTest20()
        {
            // Arrange
            var r1 = new ApplicationUser();
            var r2 = new Customer();
            var r3 = new Staff();
            var _contact1 = new ContactUser("20", "test", "test", "test", 123, "test", r1);
            var _contact2 = new ContactUser("20", "test", "test", "test", 123, "test", r2);
            var _contact3 = new ContactUser("20", "test", "test", "test", 123, "test", r3);

            // Act
            r1.addContact("20", "test", "test", "test", 123, "test", r1);
            r1.addContact(null, null, null,null,null,null,null);
            r1.addContact("20", "test", "test", "test", 123, "test", null);
            r1.addContact("20", "test", "test", "test", 123, "test", r1);

            r2.addContact("20", "test", "test", "test", 123, "test", r2);
            r2.addContact(null, null, null, null, null, null, null);
            r2.addContact("20", "test", "test", "test", 123, "test", null);
            r2.addContact("20", "test", "test", "test", 123, "test", r2);

            r3.addContact("20", "test", "test", "test", 123, "test", r3);
            r3.addContact(null, null, null, null, null, null, null);
            r3.addContact("20", "test", "test", "test", 123, "test", null);
            r3.addContact("20", "test", "test", "test", 123, "test", r3);


            // Assert
            Assert.AreEqual<string>(_contact1.PropertyNo, r1.Contact.Single().PropertyNo);
            Assert.AreEqual<string>(_contact1.Town, r1.Contact.Single().Town);
            Assert.AreEqual<ApplicationUser>(_contact1.ApplicationUser, r1.Contact.Single().ApplicationUser);

            Assert.AreEqual<string>(_contact2.PropertyNo, r2.Contact.Single().PropertyNo);
            Assert.AreEqual<string>(_contact2.Town, r2.Contact.Single().Town);
            Assert.AreEqual<ApplicationUser>(_contact2.ApplicationUser, r2.Contact.Single().ApplicationUser);

            Assert.AreEqual<string>(_contact3.PropertyNo, r3.Contact.Single().PropertyNo);
            Assert.AreEqual<string>(_contact3.Town, r3.Contact.Single().Town);
            Assert.AreEqual<ApplicationUser>(_contact3.ApplicationUser, r3.Contact.Single().ApplicationUser);
        }

        [TestMethod]
        public void ServiceApplyEditIMSTest21()
        {
            // Arrange
            var vm1 = new ServiceCreateIMSViewModel();
            var vm2 = new ServiceEditIMSViewModel();
            var vm3 = new ServiceEditIMSViewModel();

            vm1.LastModified = DateTime.Now;
            vm1.StaffOwnerID = "testFrom VM";

            vm2.StaffOwnerID = "changed";
            vm2.LastModified = DateTime.Now.AddDays(1);

            vm3.StaffOwnerID = null;
            vm3.LastModified = DateTime.Today;

            var r1 = new Service(vm1);

            // Act
            r1.ApplyEditIMS(vm2);
            r1.ApplyEditIMS(vm3);
            r1.ApplyEditIMS(vm2);

            // Assert
            Assert.AreEqual<string>(vm2.StaffOwnerID, r1.StaffOwnerID);
            Assert.AreEqual<DateTime>(vm2.LastModified, r1.LastModified);

        }

        [TestMethod]
        public void PropertyGetContactTest22()
        {
            // Arrange
            var l1 = new List<ContactProperty>();
            l1.Add(new ContactProperty("20", "test", "test", "test", 123, "test", null));
            l1.Add(new ContactProperty("30", "test2", "test2", "test2", 345, "test", null));

            var r1 = new Property();
            var r2 = new Property();

            // Act
            //Test the method with valid array (but with too many items in it)
            var cp = r1.getContact(l1);
            
            //Test the method with an empty array
            var cp2 = r2.getContact(new List<ContactProperty>());

            // Assert
            Assert.AreEqual<ContactProperty>(l1[0], cp);
            Assert.AreNotEqual<ContactProperty>(l1[1], cp);

            Assert.AreEqual<string>("No address found", cp2.PropertyNo);
        }
    }
}
