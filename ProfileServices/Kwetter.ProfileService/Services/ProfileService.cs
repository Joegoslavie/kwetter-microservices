namespace Kwetter.ProfileService.Services
{
    using Grpc.Core;
    using Kwetter.ProfileService.Business;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ProfileService : ProfileGRPCService.ProfileGRPCServiceBase
    {
        private readonly ILogger<ProfileService> logger;

        private readonly ProfileManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manager"></param>
        public ProfileService(ILogger<ProfileService> logger, ProfileManager manager)
        {
            this.logger = logger;
            this.manager = manager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ProfileResponse> GetAll(ProfileRequest request, ServerCallContext context)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override Task<ProfileResponse> GetProfileByUserId(ProfileRequest request, ServerCallContext context)
        {
            try
            {
                Task.Run(() =>
                {
                    var entity = this.manager.FindById(request.UserId);
                });
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception occurred in retrieving profile", ex);
                throw;
            }
        }

        public override Task<ProfileResponse> GetProfileByUsername(ProfileRequest request, ServerCallContext context)
        {
            try
            {
                Task.Run(() =>
                {
                    var entity = this.manager.FindByUsername(request.Username);
                });
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception occurred in retrieving profile", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ProfileResponse> CreateProfile(UpdateProfileRequest request, ServerCallContext context)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
