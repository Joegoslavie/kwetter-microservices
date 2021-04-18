using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.ProfileService.Persistence.Entity
{
    public class ProfileEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public Uri WebsiteUri { get; set; }
        public Uri AvatarUri { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
