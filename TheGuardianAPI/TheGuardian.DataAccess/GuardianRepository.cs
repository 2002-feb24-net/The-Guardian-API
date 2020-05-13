using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheGuardian.Core.Interfaces;


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

            return Mapper.MapHospital(newHospital);
        }

        /// <summary>
        /// Adds a new Review to the DB.
        /// </summary>
        /// <param name="review"></param>
        /// <returns>The review that was added</returns>
        public async Task<Core.Models.Review> PostReviewAsync(Core.Models.Review review)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == review.UserId);
            var hospital = await _dbContext.Hospitals.FirstOrDefaultAsync(h => h.Id == review.HospitalId);
            if (user is null || hospital is null)
            {
                _logger.LogInformation($"Couldn't find hospital with ID {review.HospitalId} and/or user with ID {review.UserId}.");
                return null;
            }
            /*if (!(await _dbContext.Reviews.FirstOrDefaultAsync(r => r.UserId == user.Id && r.HospitalId == hospital.Id) is null))
            {
                _logger.LogInformation($"User with id {review.UserId} already placed a review at hospital with id {review.HospitalId}.");
                return null;
            }*/
            Review newReview = new Review
            {
                UserId = review.UserId,
                HospitalId = review.HospitalId,
                MedicalStaffRating = review.MedicalStaffRating,
                ClericalStaffRating = review.ClericalStaffRating,
                FacilityRating = review.FacilityRating,
                OverallRating = (review.MedicalStaffRating + review.ClericalStaffRating + review.FacilityRating) / 3.0,
                WrittenFeedback = review.WrittenFeedback,
                Reason = review.Reason,
                ReasonOther = review.ReasonOther,
                DateAdmittance = review.DateAdmittance,
            };

            _logger.LogInformation($"Added a review from user with id {review.UserId} to hospital with id {review.HospitalId} to DB.");
            _dbContext.Reviews.Add(newReview);
            await _dbContext.SaveChangesAsync();

            var updatedHospital = await _dbContext.Hospitals.Include(h => h.Reviews).FirstOrDefaultAsync(h => h.Id == review.HospitalId);
            updatedHospital.Reviews.Add(newReview);
            var updatedUser = await _dbContext.Users.Include(r => r.Reviews).FirstOrDefaultAsync(r => r.Id == review.UserId);
            updatedHospital.Reviews.Add(newReview);
            updatedUser.Reviews.Add(newReview);

            UpdateAggregateRatings(updatedHospital); // Update the hospital's aggregate ratings after adding a new review.
            await _dbContext.SaveChangesAsync();

            var resultReview = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == newReview.Id);
            return Mapper.MapReview(resultReview);
        }

        public void UpdateAggregateRatings(Hospital hospital)
        {
            if (hospital.Reviews.Count == 0)
            {
                return;
            }
            double totalClericalRatings = 0, totalFacilityRatings = 0, totalMedicalRatings = 0;
            foreach (var review in hospital.Reviews.ToList())
            {
                totalClericalRatings += review.ClericalStaffRating;
                totalFacilityRatings += review.FacilityRating;
                totalMedicalRatings += review.MedicalStaffRating;
            }
            hospital.AggClericalStaffRating = totalClericalRatings / hospital.Reviews.Count;
            hospital.AggFacilityRating = totalClericalRatings / hospital.Reviews.Count;
            hospital.AggMedicalStaffRating = totalMedicalRatings / hospital.Reviews.Count;
            hospital.AggOverallRating = (hospital.AggClericalStaffRating + hospital.AggFacilityRating + hospital.AggMedicalStaffRating) / 3.0;
        }

        /// <summary>
        /// Adds a new User to the DB.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The user that was added</returns>
        public async Task<Core.Models.User> PostUserAsync(Core.Models.User user)
        {
            if (!(await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email) is null))
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
            var resultUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            return Mapper.MapUser(resultUser);
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
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);

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
            var reviews = await _dbContext.Reviews.ToListAsync();
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
        /// Retreiving a single user from the DB for login
        /// </summary>
        /// <returns>A single user</returns>
        public async Task<Core.Models.User> GetUserLoginAsync(string email, string password)
        {
            TheGuardian.DataAccess.User user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            return Mapper.MapUser(user);
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
            Review changedReview = new Review
            {
                Id = reviewExists.Id,
                UserId = reviewExists.UserId,
                HospitalId = reviewExists.HospitalId,
                MedicalStaffRating = review.MedicalStaffRating,
                ClericalStaffRating = review.ClericalStaffRating,
                FacilityRating = review.FacilityRating,
                OverallRating = (review.MedicalStaffRating + review.ClericalStaffRating + review.FacilityRating) / 3.0,
                DateAdmittance = review.DateAdmittance,
                DateSubmitted = reviewExists.DateSubmitted,
                WrittenFeedback = review.WrittenFeedback,
                Reason = review.Reason,
                ReasonOther = review.ReasonOther
            };
            _logger.LogInformation($"Updating review with id {id}.");
            _dbContext.Entry(reviewExists).CurrentValues.SetValues(changedReview);
            await _dbContext.SaveChangesAsync();

            var updatedHospital = await _dbContext.Hospitals.Include(h => h.Reviews).FirstOrDefaultAsync(h => h.Id == review.HospitalId);
            UpdateAggregateRatings(updatedHospital); // Update the hospital's aggregate ratings after adding a new review.
            await _dbContext.SaveChangesAsync();

            return review;
        }
    }
}