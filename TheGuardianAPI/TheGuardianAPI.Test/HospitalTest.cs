using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using TheGuardian.Core.Interfaces;
using TheGuardian.Api.Controllers;
using FluentAssertions;
using TheGuardian.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TheGuardianAPI.Test
{
    public class HospitalTest
    {
        [Fact]
        public async Task GetHospitalsTest()
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
            mockIGuardianRepository.Setup(x => x.GetHospitalsAsync()).Verifiable();
            var hospitalsController = new HospitalsController(mockIGuardianRepository.Object);
            var allHospitals = await hospitalsController.GetHospitals();
            allHospitals.Should().NotBeNull();
        }


    }

}
