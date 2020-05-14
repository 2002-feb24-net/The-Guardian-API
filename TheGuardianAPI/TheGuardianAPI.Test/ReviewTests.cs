using Xunit;
using TheGuardian.Api.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using TheGuardian.Core.Interfaces;
using FluentAssertions;
using TheGuardian.DataAccess;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace TheGuardian.Test
{
    public class ReviewTests
    {
        private Mock<IGuardianRepository> mockIRepo;
        private ReviewsController reviewsController;

        public ReviewTests()
        {
            mockIRepo = new Mock<IGuardianRepository>();
            reviewsController = new ReviewsController(mockIRepo.Object);
        }

        [Fact]
        public async void PostReviewTest()
        {
            DataAccess.Review testSubject = new DataAccess.Review
            {
                Id = 1,
                UserId = 1,
                HospitalId = 1,
                MedicalStaffRating = 5,
                ClericalStaffRating = 5,
                FacilityRating = 5,
                OverallRating = 5,
                WrittenFeedback = "Uninformative feedback.",
                Reason = "Surgery",
                DateAdmittance = DateTime.Now
            };
            mockIRepo.Setup(x => x.PostReviewAsync(Mapper.MapReview(testSubject))).Verifiable();
            var postResult = await reviewsController.PostReview(testSubject);
            postResult.Result.Should().Equals(testSubject);
        }

        [Fact]
        public async void PutReviewTest()
        {
            DataAccess.Review testSubject = new DataAccess.Review
            {
                Id = 1,
                UserId = 1,
                HospitalId = 1,
                MedicalStaffRating = 5,
                ClericalStaffRating = 5,
                FacilityRating = 5,
                OverallRating = 5,
                WrittenFeedback = "Uninformative feedback.",
                Reason = "Surgery",
                DateAdmittance = DateTime.Now
            };
            mockIRepo.Setup(x => x.PutReviewAsync(1, Mapper.MapReview(testSubject))).Verifiable();
            var putResult = await reviewsController.PutReview(1, testSubject);
            putResult.Should().Equals(testSubject);
        }


        [Fact]
        public async void GetReviewsTest()
        {
            mockIRepo.Setup(x => x.GetReviewsAsync()).Verifiable();
            var allReviews = await reviewsController.GetReviews();
            allReviews.Should().NotBeNull();
        }

        [Fact]
        public async void GetReviewTest()
        {
            mockIRepo.Setup(x => x.GetReviewAsync(-1)).Verifiable();
            var nullReview = await reviewsController.GetReview(-1);
            nullReview.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void DeleteReview()
        {
            var deleteResult = await reviewsController.DeleteReview(1);
            deleteResult.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}
