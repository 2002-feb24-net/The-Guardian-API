using Xunit;
using Moq;
using System.Threading.Tasks;
using TheGuardian.Core.Interfaces;
using TheGuardian.Api.Controllers;
using FluentAssertions;
using TheGuardian.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace TheGuardianAPI.Test
{
    public class HospitalTest
    {
        Mock<IGuardianRepository> mockIGuardianRepository;
        HospitalsController hospitalsController;

        public HospitalTest()
        {
            mockIGuardianRepository = new Mock<IGuardianRepository>();
            hospitalsController = new HospitalsController(mockIGuardianRepository.Object);
        }

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

        [Fact]
        public async void PostHospitalTest()
        {
            TheGuardian.DataAccess.Hospital listOfHospitals = new TheGuardian.DataAccess.Hospital
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
            mockIGuardianRepository.Setup(x => x.PostHospitalAsync(Mapper.MapHospital(listOfHospitals))).Verifiable();
            var postResult = await hospitalsController.PostHospital(listOfHospitals);
            postResult.Result.Should().Equals(listOfHospitals);
        }

        [Fact]
        public async void GetHospitalTest()
        {
            mockIGuardianRepository.Setup(x => x.GetHospitalAsync(-1)).Verifiable();
            var nullHospital = await hospitalsController.GetHospital(-1);
            nullHospital.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void DeleteHospital()
        {
            var deleteResult = await hospitalsController.DeleteHospital(1);
            deleteResult.Should().BeOfType<NotFoundResult>();
        }

    }

}
