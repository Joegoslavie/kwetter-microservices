namespace Kwetter.ProfileService.Persistence.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a profile record.
    /// </summary>
    public class ProfileEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets username of the profile.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the user id of the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the website url.
        /// </summary>
        public string WebsiteUri { get; set; }

        /// <summary>
        /// Gets or sets the avatar url.
        /// </summary>
        public string AvatarUri { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the datetime the record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the datetime the record was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
