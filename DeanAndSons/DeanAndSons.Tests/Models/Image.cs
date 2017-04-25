//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using DeanAndSons.Models;
//using System.Web;
//using System.IO;
//using System.Reflection;
//using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

//namespace DeanAndSons.Tests.Models
//{
//    [TestClass]
//    public class Image
//    {
//        [TestMethod]
//        [HostType("ASP.NET")]
//        [UrlToTest("http://localhost:27789/")]
//        [AspNetDevelopmentServerHost("C:\\Users\\dexte\\Source\\Repos\\DeanAndSons\\DeanAndSons\\DeanAndSons", "C:\\Users\\dexte\\Source\\Repos\\DeanAndSons\\DeanAndSons\\DeanAndSons")]
//        public void saveImageTest19()
//        {
//            // Arrange // Act
//            // Create new objects with test post codes. This will invoke the getLatLong() method
//            // where the external call to Google takes place
//            //var constructorInfo = typeof(HttpPostedFile).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];
//            //var obj = (HttpPostedFile)constructorInfo
//            //          .Invoke(new object[] { "filename", "image/jpeg", null });

//            MemoryFile m = new MemoryFile();

//            ImageProperty r1 = new ImageProperty(m, ImageType.PropertyHeader, "/Storage/Propertys", null);
//            //Image r2 = new ImageProperty("", "", "", "", null, "", null);
//            //Image r3 = new ImageProperty("", "", "", "123456789", null, "", null);
//            //Image r4 = new ImageProperty("", "", "", "1234567890", null, "", null);

//            // Assert
//            //Assert.AreNotEqual<double>(0, r1.Lat);
//            //Assert.AreNotEqual<double>(0, r1.Long);

//            //Assert.AreEqual<double>(51.375814, r2.Lat);
//            //Assert.AreEqual<double>(-2.359904, r2.Long);

//            //Assert.AreEqual<double>(51.375814, r3.Lat);
//            //Assert.AreEqual<double>(-2.359904, r3.Long);

//            //Assert.AreEqual<double>(51.375814, r4.Lat);
//            //Assert.AreEqual<double>(-2.359904, r4.Long);
//        }
//    }

//    class MemoryFile : HttpPostedFileBase
//    {
//        Stream stream;
//        string contentType;
//        string fileName;

//        public MemoryFile()
//        {
//            Stream s = File.Open("C:\\Users\\dexte\\Desktop\\Dining-Room.jpg", FileMode.Open);
            
//            this.stream = s;
//            this.contentType = "image";
//            this.fileName = "Dining-Room.jpg";
//        }

//        public override int ContentLength
//        {
//            get { return (int)stream.Length; }
//        }

//        public override string ContentType
//        {
//            get { return contentType; }
//        }

//        public override string FileName
//        {
//            get { return fileName; }
//        }

//        public override Stream InputStream
//        {
//            get { return stream; }
//        }

//        public override void SaveAs(string filename)
//        {
//            using (var file = File.Open(filename, FileMode.CreateNew))
//                stream.CopyTo(file);
//        }
//    }
//}
