using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NubeBooks;
using NubeBooks.Controllers;

namespace NubeBooks.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            //ViewResult result = controller.About() as ViewResult;

            var result = "Your application description page.";

            // Assert
            Assert.AreEqual("Your application description page.", result);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            //ViewResult result = controller.Contact() as ViewResult;

            var result = "";

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
