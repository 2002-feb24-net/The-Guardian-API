using Microsoft.EntityFrameworkCore;
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
        public async Task<Core.Models.Hospital> PutHospitalAsync(Core.Models.Hospital hospital)
        {
            int id = await _dbContext.Hospitals.MaxAsync(h => h.Id) + 1; // Get max hospital id + 1 for the new hospital.

            var newHospital = new Hospital
            {
                Id = id,
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
        public async Task<Core.Models.Review> PutReviewAsync(Core.Models.Review review)
        {
            var reviewExists = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.UserId == review.UserId && r.HospitalId == review.HospitalId);
            if (reviewExists != null)
            {
                _logger.LogInformation($"User with id {review.UserId} already placed a review at hospital with id {review.HospitalId}.");
                return null;
            }

            int id = await _dbContext.Reviews.MaxAsync(h => h.Id) + 1; // Get max review id + 1 for the new review.

            Review newReview = new Review
            {
                Id = id,
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
        public async Task<Core.Models.User> PutUserAsync(Core.Models.User user)
        {
            var userExists = _dbContext.Users.FirstOrDefaultAsync(u => user.Email == u.Email);
            if (userExists != null)
            {
                _logger.LogInformation($"User with email {user.Email} already exists.");
                return null;
            }
            int id = await _dbContext.Users.MaxAsync(h => h.Id) + 1; // Get max user id + 1 for the new user.

            User newUser = new User
            {
                Id = id,
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

        public Task<IEnumerable<Core.Models.Review>> GetReviewsAsync()
        {
            throw new NotImplementedException();
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
                return null;
            }
            _logger.LogInformation($"Fetched user with email {email}.");
            return Mapper.MapUser(user);
        }

        public Task<IEnumerable<Core.Models.User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveHospitalAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveReviewAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Models.User> PostUserAsync(Core.Models.User user)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Models.Hospital> PostHospitalAsync(Core.Models.Hospital hospital)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Models.Review> PostReviewAsync(Core.Models.Review review)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Core.Models.Reason>> GetReasonsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Core.Models.Review> GetReasonAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Models.Review> PutReasonAsync(Core.Models.Reason reason)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Models.Review> PostReasonAsync(Core.Models.Reason reason)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveReasonAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
