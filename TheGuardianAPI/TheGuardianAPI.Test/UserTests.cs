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

namespace TheGuardian.Test
{
    public class UserTests
    {
        Mock<IGuardianRepository> mockIRepo;
        UsersController usersController;

        public UserTests() 
        {
            mockIRepo = new Mock<IGuardianRepository>();
            usersController = new UsersController(mockIRepo.Object);
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
        public async void PostDeleteUserTest()
        {
            DataAccess.User testSubject = new DataAccess.User
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Subject",
                Email = "testSubect@xunit.com",
                Password = "password",
                Address = "Address",
                City = "City",
                Zip = 12345,
            };
            var postResult = await usersController.PostUser(testSubject);
            postResult.Result.Should().Equals(testSubject);

            var deleteResult = await usersController.DeleteUser(1);
            deleteResult.Result.Should().Equals(testSubject);

        }
    }
}
