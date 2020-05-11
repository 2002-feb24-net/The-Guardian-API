using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TheGuardian.DataAccess
{
    public static class Mapper
    {
        public static Core.Models.User MapUser(User user)
        {
            return user is null ? null : new Core.Models.User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Address = user.Address,
                City = user.City,
                State = user.State,
                Zip = user.Zip,
                AccessLevel = user.AccessLevel,
                AccountDate = user.AccountDate,
                AccountVerified = user.AccountVerified,
                Reviews = user.Reviews.Select(MapReview).ToList()
            };
        }

        public static User MapUser(Core.Models.User user)
        {
            return user is null ? null : new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Address = user.Address,
                City = user.City,
                State = user.State,
                Zip = user.Zip,
                AccessLevel = user.AccessLevel,
                AccountDate = user.AccountDate,
                AccountVerified = user.AccountVerified,
                Reviews = user.Reviews.Select(MapReview).ToList()
            };
        }

        public static Core.Models.Hospital MapHospital(Hospital hospital)
        {
            return hospital is null ? null : new Core.Models.Hospital
            {
                Id = hospital.Id,
                Name = hospital.Name,
                Address = hospital.Address,
                City = hospital.City,
                State = hospital.State,
                Zip = hospital.Zip,
                Phone = hospital.Phone,
                Website = hospital.Website,
                AggMedicalStaffRating = hospital.AggMedicalStaffRating,
                AggClericalStaffRating = hospital.AggClericalStaffRating,
                AggFacilityRating = hospital.AggFacilityRating,
                AggOverallRating = hospital.AggOverallRating,
                Reviews = hospital.Reviews.Select(MapReview).ToList()
            };
        }

        public static Hospital MapHospital(Core.Models.Hospital hospital)
        {
            return hospital is null ? null : new Hospital
            {
                Id = hospital.Id,
                Name = hospital.Name,
                Address = hospital.Address,
                City = hospital.City,
                State = hospital.State,
                Zip = hospital.Zip,
                Phone = hospital.Phone,
                Website = hospital.Website,
                AggMedicalStaffRating = hospital.AggMedicalStaffRating,
                AggClericalStaffRating = hospital.AggClericalStaffRating,
                AggFacilityRating = hospital.AggFacilityRating,
                AggOverallRating = hospital.AggOverallRating,
                Reviews = hospital.Reviews.Select(MapReview).ToList()
            };
        }

        public static Core.Models.Review MapReview(Review review)
        {
            return review is null ? null : new Core.Models.Review
            {
                Id = review.Id,
                UserId = review.UserId,
                HospitalId = review.HospitalId,
                MedicalStaffRating = review.MedicalStaffRating,
                ClericalStaffRating = review.ClericalStaffRating,
                FacilityRating = review.FacilityRating,
                OverallRating = review.OverallRating,
                WrittenFeedback = review.WrittenFeedback,
                DateSubmitted = review.DateSubmitted,
                DateAdmittance = review.DateAdmittance,
                Reason = review.Reason,
                User = MapUser(review.User),
                Hospital = MapHospital(review.Hospital)
            };
        }

        public static Review MapReview(Core.Models.Review review)
        {
            return review is null ? null : new Review
            {
                Id = review.Id,
                UserId = review.UserId,
                HospitalId = review.HospitalId,
                MedicalStaffRating = review.MedicalStaffRating,
                ClericalStaffRating = review.ClericalStaffRating,
                FacilityRating = review.FacilityRating,
                OverallRating = review.OverallRating,
                WrittenFeedback = review.WrittenFeedback,
                DateSubmitted = review.DateSubmitted,
                DateAdmittance = review.DateAdmittance,
                Reason = review.Reason,
                User = MapUser(review.User),
                Hospital = MapHospital(review.Hospital)
            };
        }
    }
}
