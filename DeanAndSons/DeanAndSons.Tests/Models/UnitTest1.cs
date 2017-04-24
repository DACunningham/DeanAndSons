﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeanAndSons.Models;

namespace DeanAndSons.Tests.Models
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetLocationLatLongTest02()
        {
            // Arrange

            // Act
            var r1 = Contact.GetLocationLatLng("Chippenham");
            var r2 = Contact.GetLocationLatLng(null);
            var r3 = Contact.GetLocationLatLng("nfuire;laugfd;sagfds56g43sdg879fr6ts43gr");

            // Assert
            Assert.AreEqual<double>(r1.Latitude, 51.461552);
            Assert.AreEqual<double>(r1.Longitude, -2.119497);

            Assert.AreEqual<double>(r2.Latitude, 51.375814);
            Assert.AreEqual<double>(r2.Longitude, -2.359904);

            Assert.AreEqual<double>(r3.Latitude, 51.375814);
            Assert.AreEqual<double>(r3.Longitude, -2.359904);
        }

        [TestMethod]
        public void GetLatLongTest03()
        {
            // Arrange // Act
            Contact r1 = new ContactProperty("", "", "", "WD19 5AX", null, "", null);

            Contact r2 = new ContactProperty("", "", "", "", null, "", null);

            Contact r3 = new ContactProperty("", "", "", "123456789", null, "", null);

            Contact r4 = new ContactProperty("", "", "", "1234567890", null, "", null);

            // Assert
            Assert.AreNotEqual<double>(0, r1.Lat);
            Assert.AreNotEqual<double>(0, r1.Long);

            Assert.AreEqual<double>(0, r2.Lat);
            Assert.AreEqual<double>(0, r2.Long);

            Assert.AreEqual<double>(0, r3.Lat);
            Assert.AreEqual<double>(0, r3.Long);

            Assert.AreEqual<double>(0, r4.Lat);
            Assert.AreEqual<double>(0, r4.Long);
        }
    }
}
