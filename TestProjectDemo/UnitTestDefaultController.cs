using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcMovie.Controllers;

namespace TestProjectDemo
{
    [TestClass]
    public class UnitTestDefaultController
    {

        [TestMethod]
        public void TestMethod_Success()
        {
            // arrange
            var controller = new DefaultController(null);

            //act
            var result = controller.testSuccess();
            var okResult = result as ObjectResult;

            //assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void TestMethod_NotFound()
        {
            // arrange
            var controller = new DefaultController(null);

            //act
            var result = controller.testNotFound();
            var okResult = result as ObjectResult;

            //assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(404, okResult.StatusCode);
        }

        [TestMethod]
        public void TestMethod_Error()
        {
            // arrange
            var controller = new DefaultController(null);

            //act
            var result = controller.testError();
            var okResult = result as ObjectResult;

            //assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(500, okResult.StatusCode);
        }

    }
}
