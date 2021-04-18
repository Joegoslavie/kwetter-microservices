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

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manager"></param>
        public ProfileService(ILogger<ProfileService> logger, ProfileManager manager)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ProfileResponse> GetAll(ProfileRequest request, ServerCallContext context)
        {
            return base.GetAll(request, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ProfileResponse> GetProfile(ProfileRequest request, ServerCallContext context)
        {
            return base.GetProfile(request, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ProfileResponse> CreateProfile(UpdateProfileRequest request, ServerCallContext context)
        {
            return base.CreateProfile(request, context);
        }
    }
}
