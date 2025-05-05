using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCI3110TermProject.Data
{
    /// <summary>
    /// Represents a single job application record,
    /// including company, position, date applied, and any tags.
    /// </summary>
    public class JobApplication
    {
        /// <summary>
        /// Primary key for the job application.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the company where the application was submitted.
        /// </summary>
        [Required, MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        /// The position or role being applied for.
        [Required, MaxLength(100)]
        public string Position { get; set; } = string.Empty;

        /// Date when the application was sent.
        [Required]
        public DateTime DateApplied { get; set; }

        /// <summary>
        /// Navigation property for linking tags (many-to-many).
        /// Initialized to avoid null-reference issues.
        /// </summary>
        public ICollection<JobApplicationTag> JobApplicationTags { get; set; }
            = new List<JobApplicationTag>();
    }

}
