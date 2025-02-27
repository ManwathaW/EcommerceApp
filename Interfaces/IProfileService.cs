using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Interfaces
{
    public interface IProfileService
    {
        Task<Profile> GetProfileAsync();
        Task<bool> SaveProfileAsync(Profile profile);
        Task<string> SaveProfileImageAsync(FileResult image);
        Task<bool> DeleteProfileImageAsync(string imagePath);
    }
}
