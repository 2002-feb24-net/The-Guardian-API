using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheGuardian.Core.Interfaces;
using TheGuardian.Api.Controllers;
using TheGuardianAPI;
using TheGuardian.Core.Models;
using FluentAssertions;
using TheGuardian.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TheGuardianAPI.Test
{
    [TestClass]
    public class HospitalTest
    {
        [TestMethod]
        public void GetHospitals()
        {
            var listOfHospitals = new TheGuardian.Core.Models.Hospital();
            listOfHospitals = new TheGuardian.Core.Models.Hospital
            {
                Id = 1,
                Name = "TestName",
                Address = "TestAddress",
                City = "City",
                State = "State",
                Zip = 11010,
                Phone = "5164911125",
                Website = "TestHospital.com",
                AggClericalStaffRating = 5,
                AggFacilityRating = 5,
                AggMedicalStaffRating = 5,
                AggOverallRating = 5,

            };

            var a = Mapper.MapHospital(listOfHospitals);
            Mock<IGuardianRepository> mockIGuardianRepository = new Mock<IGuardianRepository>();
            mockIGuardianRepository.Setup(x => x.GetUsersAsync()).Verifiable();
            var hospitalsController = new HospitalsController(mockIGuardianRepository.Object);
            hospitalsController.Should().NotBeNull();
        }


    }

}
