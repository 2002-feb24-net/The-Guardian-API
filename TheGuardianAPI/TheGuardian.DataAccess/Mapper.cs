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
            return new Core.Models.User
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
            return new Core.Models.Hospital
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
            return new Core.Models.Review
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
                ReasonId = review.ReasonId,
                Reason = MapReason(review.Reason),
                User = MapUser(review.User),
                Hospital = MapHospital(review.Hospital)
            };
        }

        public static Core.Models.Reason MapReason(Reason reason)
        {
            return new Core.Models.Reason
            {
                Id = reason.Id,
                ReasonDescription = reason.ReasonDescription
            };
        }
    }
}
