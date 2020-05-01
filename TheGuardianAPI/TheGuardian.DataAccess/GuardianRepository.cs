using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGuardian.Core.Interfaces;
using TheGuardian.Core.Models;

namespace TheGuardian.DataAccess
{
    public class GuardianRepository : IGuardianRepository
    {
        private readonly GuardianContext _dbContext;

        private readonly ILogger<GuardianRepository> _logger;

        /// <summary>
        /// Inistializes a new Guardian repository.
        /// </summary>
        /// <param name="dbContext">That data source</param>
        /// <param name="logger">the logger</param>
        public GuardianRepository(GuardianContext dbContext, ILogger<GuardianRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new Hospital to the DB.
        /// </summary>
        /// <param name="hospital"></param>
        /// <returns>The hospital that was added</returns>
        public async Task<Core.Models.Hospital> PostHospitalAsync(Core.Models.Hospital hospital)
        {
            var newHospital = new Hospital
            {
                Name = hospital.Name,
                Address = hospital.Address,
                City = hospital.City,
                State = hospital.State,
                Zip = hospital.Zip,
                Phone = hospital.Phone,
                Website = hospital.Website
            };

            _logger.LogInformation($"Added hospital with name {hospital.Name} to DB.");

            _dbContext.Hospitals.Add(newHospital);
            await _dbContext.SaveChangesAsync();

            int nid = await _dbContext.Hospitals.MaxAsync(h => h.Id);
            var nh = await _dbContext.Hospitals.FirstOrDefaultAsync(h => h.Id == nid);

            return Mapper.MapHospital(nh);
        }

        /// <summary>
        /// Adds a new Review to the DB.
        /// </summary>
        /// <param name="review"></param>
        /// <returns>The review that was added</returns>
        public async Task<Core.Models.Review> PostReviewAsync(Core.Models.Review review)
        {
            var reviewExists = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.UserId == review.UserId && r.HospitalId == review.HospitalId);
            if (reviewExists != null)
            {
                _logger.LogInformation($"User with id {review.UserId} already placed a review at hospital with id {review.HospitalId}.");
                return null;
            }

            Review newReview = new Review
            {
                UserId = review.UserId,
                HospitalId = review.HospitalId,
                MedicalStaffRating = review.MedicalStaffRating,
                ClericalStaffRating = review.ClericalStaffRating,
                FacilityRating = review.FacilityRating,
                OverallRating = review.OverallRating,
                WrittenFeedback = review.WrittenFeedback,
                ReasonId = review.ReasonId,
                DateAdmittance = review.DateAdmittance
            };

            _logger.LogInformation($"Added a review from user with id {review.UserId} to hospital with id {review.HospitalId} to DB.");

            _dbContext.Reviews.Add(newReview);
            await _dbContext.SaveChangesAsync();

            var hospital = await _dbContext.Hospitals.Include(h => h.Reviews).FirstOrDefaultAsync(h => h.Id == newReview.HospitalId);
            hospital.UpdateAggregateRatings(); // Update the hospital's aggregate ratings after adding a new review.
            await _dbContext.SaveChangesAsync();

            int nid = await _dbContext.Reviews.MaxAsync(r => r.Id);
            var nr = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == nid);

            return Mapper.MapReview(nr);
        }

        /// <summary>
        /// Adds a new User to the DB.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The user that was added</returns>
        public async Task<Core.Models.User> PostUserAsync(Core.Models.User user)
        {
            var userExists = _dbContext.Users.FirstOrDefaultAsync(u => user.Email == u.Email);
            if (userExists != null)
            {
                _logger.LogInformation($"User with email {user.Email} already exists.");
                return null;
            }

            User newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Address = user.Address,
                City = user.City,
                State = user.State,
                Zip = user.Zip
            };

            _logger.LogInformation($"Added a User with email {user.Email} to DB.");

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            int nid = await _dbContext.Users.MaxAsync(u => u.Id);
            var nu = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == nid);

            return Mapper.MapUser(nu);
        }

        /// <summary>
        /// Gets hospital with reviews
        /// based on a hospital's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A hospital</returns>
        public async Task<Core.Models.Hospital> GetHospitalAsync(int id)
        {
            var hospital = await _dbContext.Hospitals.Include(h => h.Reviews).FirstOrDefaultAsync(h => h.Id == id);

            if (hospital == null)
            {
                _logger.LogInformation($"Hospital with id {id} not found.");
                return null;
            }
            _logger.LogInformation($"Fetched all hospital with id {id}.");
            return Mapper.MapHospital(hospital);
        }

        /// <summary>
        /// Retreiving all hospitals with their reviews
        /// from the DB
        /// </summary>
        /// <returns>A List of hospitals</returns>
        public async Task<IEnumerable<Core.Models.Hospital>> GetHospitalsAsync()
        {
            var hospitals = await _dbContext.Hospitals.Include(h => h.Reviews).ToListAsync();
            _logger.LogInformation("Fetched all hospitals.");
            return hospitals.Select(Mapper.MapHospital);
        }

        /// <summary>
        /// Gets review with associated hospital and user
        /// based on a review's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A review</returns>
        public async Task<Core.Models.Review> GetReviewAsync(int id)
        {
            var review = await _dbContext.Reviews.Include(r => r.Hospital).Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                _logger.LogInformation($"Review with id {id} not found.");
                return null;
            }

            return Mapper.MapReview(review);
        }

        /// <summary>
        /// Retreive all reviews with their hospitals and users.
        /// from the DB
        /// </summary>
        /// <returns>A List of reviews</returns>
        public async Task<IEnumerable<Core.Models.Review>> GetReviewsAsync()
        {
            var reviews = await _dbContext.Reviews.Include(r => r.Hospital).Include(r => r.User).ToListAsync();
            _logger.LogInformation("Fetched all reviews.");
            return reviews.Select(Mapper.MapReview);
        }

        /// <summary>
        /// Get a user by ID asynchronously
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A single user with id</returns>
        public async Task<Core.Models.User> GetUserAsync(int id)
        {
            var user = await _dbContext.Users.Include(u => u.Reviews).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                _logger.LogInformation($"User with id {id} not found.");
                return null;
            }
            _logger.LogInformation($"Fetched user with id {id}.");
            return Mapper.MapUser(user);
        }

        /// <summary>
        /// Get a user by Email asynchronously
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A single user with email</returns>
        public async Task<Core.Models.User> GetUserAsync(string email)
        {
            var user = await _dbContext.Users.Include(u => u.Reviews).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                _logger.LogInformation($"User with email {email} not found.");
                return null;
            }
            _logger.LogInformation($"Fetched user with email {email}.");
            return Mapper.MapUser(user);
        }

        /// <summary>
        /// Retreiving all users with their reviews
        /// from the DB
        /// </summary>
        /// <returns>A List of users</returns>
        public async Task<IEnumerable<Core.Models.User>> GetUsersAsync()
        {
            var users = await _dbContext.Users.Include(u => u.Reviews).ToListAsync();
            _logger.LogInformation("Fetched all users.");
            return users.Select(Mapper.MapUser);
        }

        /// <summary>
        /// Removing Hospital and associated reviews from DB based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A bool based on success/failure</returns>
        public async Task<bool> RemoveHospitalAsync(int id)
        {
            var hospital = await _dbContext.Hospitals.Include(h => h.Reviews).FirstOrDefaultAsync(h => h.Id == id);
            if (hospital == null)
            {
                _logger.LogInformation($"Hospital with id {id} not found.");
                return false;
            }
            _logger.LogInformation($"Removing hospital with id {id} and all associated reviews from the DB.");
            foreach (var review in hospital.Reviews)
            {
                _dbContext.Remove(review);
            }
            _dbContext.Remove(hospital);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Removing  Review from DB based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A bool based on success/failure</returns>
        public async Task<bool> RemoveReviewAsync(int id)
        {
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
            if (review == null)
            {
                _logger.LogInformation($"Review with id {id} not found.");
                return false;
            }
            _logger.LogInformation($"Removing review with {id} from the DB.");
            _dbContext.Remove(review);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Removing User and associated reviews from DB based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A bool based on success/failure</returns>
        public async Task<bool> RemoveUserAsync(int id)
        {
            var user = await _dbContext.Users.Include(h => h.Reviews).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                _logger.LogInformation($"User with id {id} not found.");
                return false;
            }
            _logger.LogInformation($"Removing user with id {id} and all associated reviews from the DB.");
            foreach (var review in user.Reviews)
            {
                _dbContext.Remove(review);
            }
            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="users"></param>
        /// <returns>Returns the user after updated</returns>
        public async Task<Core.Models.User> PutUserAsync(int id, Core.Models.User user)
        {
            var userExists = await _dbContext.Users.FindAsync(id);
            if (userExists == null)
            {
                _logger.LogInformation($"Unable to update user with id {id} because it was not found.");
                return null;
            }
            _logger.LogInformation($"Updating user with id {id}.");
            _dbContext.Entry(userExists).CurrentValues.SetValues(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Update a hospital
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hospital"></param>
        /// <returns>Returns the hospital after updated</returns>
        public async Task<Core.Models.Hospital> PutHospitalAsync(int id, Core.Models.Hospital hospital)
        {
            var hospitalExists = await _dbContext.Hospitals.FindAsync(id);
            if (hospitalExists == null)
            {
                _logger.LogInformation($"Unable to update hospital with id {id} because it was not found.");
                return null;
            }
            _logger.LogInformation($"Updating hospital with id {id}.");
            _dbContext.Entry(hospitalExists).CurrentValues.SetValues(hospital);
            await _dbContext.SaveChangesAsync();
            return hospital;
        }

        /// <summary>
        /// Update a review
        /// </summary>
        /// <param name="id"></param>
        /// <param name="review"></param>
        /// <returns>Returns the review after updated</returns>
        public async Task<Core.Models.Review> PutReviewAsync(int id, Core.Models.Review review)
        {
            var reviewExists = await _dbContext.Reviews.FindAsync(id);
            if (reviewExists == null)
            {
                _logger.LogInformation($"Unable to update review with id {id} because it was not found.");
                return null;
            }
            _logger.LogInformation($"Updating review with id {id}.");
            _dbContext.Entry(reviewExists).CurrentValues.SetValues(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }

        /// <summary>
        /// Retreiving all reasons from the DB
        /// </summary>
        /// <returns>A List of reasons</returns>
        public async Task<IEnumerable<Core.Models.Reason>> GetReasonsAsync()
        {
            var reasons = await _dbContext.Reasons.ToListAsync();
            _logger.LogInformation("Fetched all reasons.");
            return reasons.Select(Mapper.MapReason);
        }

        /// <summary>
        /// Retreives a reason based on id from the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A reason</returns>
        public async Task<Core.Models.Reason> GetReasonAsync(int id)
        {
            var reason = await _dbContext.Reasons.FirstOrDefaultAsync(r => r.Id == id);
            if (reason == null)
            {
                _logger.LogInformation($"Reason with id {id} not found.");
                return null;
            }
            _logger.LogInformation($"Fetched reason with id {id}.");
            return Mapper.MapReason(reason);
        }

        /// <summary>
        /// Update a reason
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns>Returns the reason after updated</returns>
        public async Task<Core.Models.Reason> PutReasonAsync(int id, Core.Models.Reason reason)
        {
            var reasonExists = await _dbContext.Reasons.FindAsync(id);
            if (reasonExists == null)
            {
                _logger.LogInformation($"Unable to update reason with id {id} because it was not found.");
                return null;
            }
            _logger.LogInformation($"Updating reason with id {id}.");
            _dbContext.Entry(reasonExists).CurrentValues.SetValues(reason);
            await _dbContext.SaveChangesAsync();
            return reason;
        }

        /// <summary>
        /// Adds a new reason to the DB
        /// </summary>
        /// <param name="reason"></param>
        /// <returns>The reason added</returns>
        public async Task<Core.Models.Reason> PostReasonAsync(Core.Models.Reason reason)
        {
            var newReason = new Reason
            {
                ReasonDescription = reason.ReasonDescription
            };

            _logger.LogInformation($"Added reason with description {reason.ReasonDescription} to DB.");

            _dbContext.Reasons.Add(newReason);
            await _dbContext.SaveChangesAsync();

            int nid = await _dbContext.Reasons.MaxAsync(r => r.Id);
            var nh = await _dbContext.Reasons.FirstOrDefaultAsync(r => r.Id == nid);

            return Mapper.MapReason(nh);
        }

        /// <summary>
        /// Removing Reason from DB based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A bool based on success/failure</returns>
        public async Task<bool> RemoveReasonAsync(int id)
        {
            var reason = await _dbContext.Reasons.FirstOrDefaultAsync(r => r.Id == id);
            if (reason== null)
            {
                _logger.LogInformation($"Reason with id {id} not found.");
                return false;
            }
            _logger.LogInformation($"Removing reason with {id} from the DB.");
            _dbContext.Remove(reason);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}