using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCI3110TermProject.Data
{
    /// <summary>
    /// Represents a tag that can be attached to job applications 
    /// (for example: "Applied", "Interviewed", "Offer Received").
    /// </summary>
    public class Tag
    {
        // Primary key for the Tag entity.
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property for the many-to-many relationship 
        /// between JobApplication and Tag.
        /// Initialized here to prevent null-reference issues.
        /// </summary>
        public ICollection<JobApplicationTag> JobApplicationTags { get; set; }
            = new List<JobApplicationTag>();
    }
}
