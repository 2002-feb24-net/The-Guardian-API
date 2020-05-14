using Xunit;
using TheGuardian.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheGuardian.Core.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TheGuardian.Core.Models;
using TheGuardian.DataAccess;

namespace TheGuardian.Test
{
    public class UserTests
    {
        private Mock<IGuardianRepository> mockIRepo;
        private UsersController usersController;

        public UserTests() 
        {
            mockIRepo = new Mock<IGuardianRepository>();
            usersController = new UsersController(mockIRepo.Object);
        }

        [Fact]
        public async void PostUserTest()
        {
            DataAccess.User testSubject = new DataAccess.User
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Subject",
                Email = "testSubject@xunit.com",
                Password = "password",
                Address = "Address",
                City = "City",
                Zip = 12345,
            };
            mockIRepo.Setup(x => x.PostUserAsync(Mapper.MapUser(testSubject))).Verifiable();
            var postResult = await usersController.PostUser(testSubject);
            postResult.Result.Should().Equals(testSubject);
        }

        [Fact]
        public async void PutUserTest()
        {
            DataAccess.User testSubject = new DataAccess.User
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Subject 2",
                Email = "testSubject@xunit.com",
                Password = "password2",
                Address = "Address2",
                City = "City",
                Zip = 12345,
            };
            mockIRepo.Setup(x => x.PutUserAsync(1, Mapper.MapUser(testSubject))).Verifiable();
            var putResult = await usersController.PutUser(1, testSubject);
            putResult.Should().Equals(testSubject);
        }


        [Fact]
        public async void GetUsersTest()
        {
            mockIRepo.Setup(x => x.GetUsersAsync()).Verifiable();
            var allUsers = await usersController.GetUsers();
            allUsers.Should().NotBeNull();
        }

        [Fact]
        public async void GetUserTest()
        {
            mockIRepo.Setup(x => x.GetUserAsync(-1)).Verifiable();
            ActionResult<DataAccess.User> nullUser = await usersController.GetUser(-1);
            nullUser.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void GetUserLoginTest()
        {
            mockIRepo.Setup(x => x.GetUserLoginAsync("testSubject@xunit.com", "password")).Verifiable();
            var fetchedUser = await usersController.GetUser("testSjubect@xunit.com", "password");
            fetchedUser.Result.Should().BeOfType<NotFoundResult>();
        }


        [Fact]
        public async void DeleteUser()
        {
            var deleteResult = await usersController.DeleteUser(1);
            deleteResult.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}
