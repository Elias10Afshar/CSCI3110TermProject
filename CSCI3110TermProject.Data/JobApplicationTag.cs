using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCI3110TermProject.Data
{
    /// <summary>
    /// My many-to-many join entity between JobApplication and Tag.
    /// Each instance ties one JobApplication to one Tag.
    /// </summary>
    public class JobApplicationTag
    {
        // The primary key value of the related JobApplication.
        public int JobApplicationId { get; set; }

        // Navigation property back to the JobApplication.
        public JobApplication JobApplication { get; set; }

        // The primary key value of the related Tag.
        public int TagId { get; set; }

        // Navigation property back to the Tag.
        public Tag Tag { get; set; }
    }
}
