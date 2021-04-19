namespace Kwetter.ProfileService.Business
{
    using Kwetter.ProfileService.Persistence.Context;
    using Kwetter.ProfileService.Persistence.Entity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileManager
    {
        private readonly ProfileContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileManager"/> class.
        /// </summary>
        /// <param name="context">Profile context class.</param>
        public ProfileManager(ProfileContext context)
        {
            this.context = context;
        }

        public ProfileDto FindById(int userId)
        {
            return null;
        }

        public ProfileDto FindByUsername(string username)
        {
            return null;
        }

        public ProfileDto New(int userId, string displayName, string siteUrl, string avatarUrl, string description)
        {
            return null;
        }
    }
}
