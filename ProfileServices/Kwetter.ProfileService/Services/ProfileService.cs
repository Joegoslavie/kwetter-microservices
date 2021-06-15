namespace Kwetter.ProfileService.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Kwetter.Messaging.Interfaces.Tweet;
    using Kwetter.ProfileService.Extentions;
    using Kwetter.ProfileService.Persistence.Context;
    using Microservice.ProfileGRPCService;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Profile GRPC service.
    /// </summary>
    public class ProfileService : ProfileGRPCService.ProfileGRPCServiceBase
    {
        /// <summary>
        /// Logger instance.
        /// </summary>
        private readonly ILogger<ProfileService> logger;

        /// <summary>
        /// Profile manager instance.
        /// </summary>
        private readonly ProfileContext context;

        private readonly ITweetUpdateEvent tweetProfileUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class.
        /// </summary>
        /// <param name="logger">Injected logger.</param>
        /// <param name="context">Injected context.</param>
        public ProfileService(ILogger<ProfileService> logger, ProfileContext context, ITweetUpdateEvent tweetProfileEvent)
        {
            this.logger = logger;
            this.context = context;
            this.tweetProfileUpdate = tweetProfileEvent;
        }

        /// <summary>
        /// Retrieves a profile by user id.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="SingleProfileResponse"/>.</returns>
        public override async Task<SingleProfileResponse> GetProfileById(ProfileRequest request, ServerCallContext context)
        {
            return await Task.Run(() =>
            {
                var profile = this.context.Profiles.FirstOrDefault(p => p.UserId == request.UserId);
                if (profile == null)
                {
                    this.logger.LogWarning("Passed user id was not found", request.UserId);
                    return new SingleProfileResponse { Status = false, Message = "Profile not found by id" };
                }

                return new SingleProfileResponse
                {
                    Status = true,
                    Profile = profile.Convert(),
                };
            });
        }

        /// <summary>
        /// Retrieves multiple profiles by id.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="MultipleProfileResponse"/>.</returns>
        public override async Task<MultipleProfileResponse> GetMultipleById(ProfileRequest request, ServerCallContext context)
        {
            return await Task.Run(() =>
            {
                var profiles = this.context.Profiles.Where(p => request.UserIds.Contains(p.Id)).ToList();

                var response = new MultipleProfileResponse { Status = profiles.Any() };
                response.Profiles.AddRange(profiles.Select(x => x.Convert()));

                return response;
            })
            .ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a profile based on the username.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="SingleProfileResponse"/>.</returns>
        public override async Task<SingleProfileResponse> GetProfileByUsername(ProfileRequest request, ServerCallContext context)
        {
            return await Task.Run(() =>
            {
                var profile = this.context.Profiles.FirstOrDefault(p => p.Username == request.Username);
                if (profile == null)
                {
                    this.logger.LogWarning("Passed username was not found", request.UserId);
                    return new SingleProfileResponse { Status = false, Message = "Profile not found by username" };
                }

                return new SingleProfileResponse
                {
                    Status = true,
                    Profile = profile.Convert(),
                };
            });
        }

        /// <summary>
        /// Creates or updates the profile of the user, in case no profile entity is associated with the user id and username, a new profile will
        /// be created from the passed values.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="context">Callback context.</param>
        /// <returns><see cref="SingleProfileResponse"/>.</returns>
        public override async Task<SingleProfileResponse> UpdateProfile(UpdateProfileRequest request, ServerCallContext context)
        {
            var profile = this.context.Profiles.FirstOrDefault(p => p.UserId == request.UserId && p.Username == request.Username);
            if (profile == null)
            {
                // Profile does not exist, lets make one.
                profile = new Persistence.Entity.ProfileEntity()
                {
                    Id = request.UserId,
                    Username = request.Username,
                };

                this.logger.LogInformation($"Created new profile entity for user {request.Username}");
            }

            // Update properties.
            profile.DisplayName = request.DisplayName;
            profile.Description = request.Description;
            profile.AvatarUri = request.AvatarUri;
            profile.WebsiteUri = request.WebsiteUri;
            profile.Location = request.Location;
            profile.UpdatedAt = DateTime.Now;

            this.context.Profiles.Update(profile);
            await this.context.SaveChangesAsync().ConfigureAwait(false);

            this.tweetProfileUpdate.Invoke(profile.UserId, profile.Username, profile.DisplayName, profile.AvatarUri);
            return new SingleProfileResponse
            {
                Status = true,
                Profile = profile.Convert(),
            };
        }
    }
}
