using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheGuardian.Core.Models;

namespace TheGuardian.Core.Interfaces
{
    public interface IGuardianRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserAsync(int id);

        Task<User> PutUserAsync(int id, User user);

        Task<User> PostUserAsync(User user);

        Task<bool> RemoveUserAsync(int id);

        Task<IEnumerable<Hospital>> GetHospitalsAsync();

        Task<Hospital> GetHospitalAsync(int id);

        Task<Hospital> PutHospitalAsync(int id, Hospital hospital);

        Task<Hospital> PostHospitalAsync(Hospital hospital);

        Task<bool> RemoveHospitalAsync(int id);

        Task<IEnumerable<Review>> GetReviewsAsync();

        Task<Review> GetReviewAsync(int id);

        Task<Review> PutReviewAsync(int id, Review review);

        Task<Review> PostReviewAsync(Review review);

        Task<bool> RemoveReviewAsync(int id);

    }
}
