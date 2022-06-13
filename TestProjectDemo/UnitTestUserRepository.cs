using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Controllers;
using quest_web.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using quest_web.DAL;
using quest_web.Repository;

namespace TestProjectDemo
{
    [TestClass]
    public class UnitTestUserRepository
    {

        [TestMethod]
        public void TestMethod_AddUser()
        {
            var data = new List<User>
            {
                new User { Username = "BBB" },
                new User { Username = "ZZZ" },
                new User { Username = "AAA" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var newUser = new User {
                Username = "test",
                Updated_Date = DateTime.Now,
                Creation_Date = DateTime.Now,
                ID = 1,
                Password = "testPassword",
                Role = 0
            };

            var mockContext = new Mock<APIDbContext>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);

            var service = new UserRepository(mockContext.Object);
            service.AddUser(newUser);


            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void TestMethod_GetAllUsers()
        {
            var data = new List<User>
            {
                new User { Username = "BBB" },
                new User { Username = "ZZZ" },
                new User { Username = "AAA" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<APIDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UserRepository(mockContext.Object);
            var users = service.getAllUser();

            Assert.AreEqual(3, users.Count);
            Assert.AreEqual("BBB", users[0].Username);
            Assert.AreEqual("ZZZ", users[1].Username);
            Assert.AreEqual("AAA", users[2].Username);
        }


        [TestMethod]
        public void TestMethod_GetInfoUser()
        {
            var data = new List<User>
            {
                new User { Username = "BBB", Password = "tutu" },
                new User { Username = "ZZZ" },
                new User { Username = "AAA" },
            }.AsQueryable();



            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<APIDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UserRepository(mockContext.Object);
            var user = service.getInfoUser("BBB");

            Assert.AreEqual("tutu", user.Password);
            Assert.AreEqual("BBB", user.Username);
        }


        [TestMethod]
        public void TestMethod_GetInfoUserById()
        {
            var data = new List<User>
            {
                new User { Username = "BBB", Password = "tutu", ID = 1 },
                new User { Username = "ZZZ" },
                new User { Username = "AAA" },
            }.AsQueryable();



            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<APIDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UserRepository(mockContext.Object);
            var user = service.getInfoUserById(1);

            Assert.AreEqual("tutu", user.Password);
            Assert.AreEqual("BBB", user.Username);
        }
    }

}