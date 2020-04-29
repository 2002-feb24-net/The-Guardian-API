using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheGuardian.Core.Models;

namespace TheGuardian.Core.Interfaces
{
    interface IGuardianRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserAsync(int id);

        Task<User> GetUserAsync(string email);

        Task<User> AddUserAsync(User user);

        Task<bool> RemoveUserAsync(int id);

        Task<IEnumerable<Hospital>> GetHospitalsAsync();

        Task<Hospital> GetHospitalAsync(int id);

        Task<Hospital> AddHospitalAsync(Hospital hospital);

        Task<bool> RemoveHospitalAsync(int id);

        Task<IEnumerable<Review>> GetReviewsAsync();

        Task<Review> GetReviewAsync(int id);

        Task<Review> AddReviewAsync(Review review);

        Task<bool> RemoveReviewAsync(int id);

    }
}
