using Kwetter.ProfileService.Persistence.Entity;
using Microservice.ProfileGRPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.ProfileService.Extentions
{
    public static class ProfileExtentions
    {
        public static ProfileResponse Convert(this ProfileEntity profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            return new ProfileResponse
            {
                UserId = profile.UserId,
                Username = profile.Username,
                Description = profile.Description ?? string.Empty,
                DisplayName = profile.DisplayName,
                AvatarUri = profile.AvatarUri ?? string.Empty,
                WebsiteUri = profile.WebsiteUri ?? string.Empty,
                Location = profile.Location ?? string.Empty,
                CreatedAt = profile.CreatedAt.Ticks,
            };
        }
    }
}
