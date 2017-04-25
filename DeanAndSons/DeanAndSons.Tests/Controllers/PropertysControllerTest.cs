using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeanAndSons;
using DeanAndSons.Controllers;
using System.Web;
using Moq;
using System.Security.Principal;
using System.Web.Routing;

namespace DeanAndSons.Tests.Controllers
{
    [TestClass]
    public class PropertysControllerTest
    {
        private Mock<HttpContextBase> moqContext;
        private Mock<HttpRequestBase> moqRequest;
        [TestInitialize]
        public void SetupTests()
        {
            // Setup Moq
            moqContext = new Mock<HttpContextBase>();
            moqRequest = new Mock<HttpRequestBase>();
            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);
            
        }

        [TestMethod]
        public void EditCMS()
        {
            // Arrange
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.IsInRole("admin")).Returns(true);
            moqContext.SetupGet(ctx => ctx.User)
                   .Returns(userMock.Object);

            PropertysController controller = new PropertysController();
            
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(moqContext.Object);

            controller.ControllerContext = controllerContextMock.Object;
            //var parameters = new SubscribeParameter();

            // Act
            //ViewResult result = controller.EditCMS(10) as ViewResult;
            ViewResult result = controller.Index(null, null, "0", "0", null, "Bath", "50000", "1", "30", "10000000", "0", "0") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            userMock.Verify(p => p.IsInRole("admin"));
            Assert.AreEqual(((ViewResult)result).ViewName, "Index");
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
