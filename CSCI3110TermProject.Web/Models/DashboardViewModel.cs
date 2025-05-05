namespace CSCI3110TermProject.Web.Models
{
    /// <summary>
    /// ViewModel for the dashboard, containing overall stats
    /// about job applications and their distribution by tag.
    /// </summary>
    public class DashboardViewModel
    {
        // Total number of job applications in the system.
        public int TotalApplications { get; set; }

        /// <summary>
        /// Mapping from tag name (e.g., "Interviewed", "Offer")
        /// to the count of applications labeled with that tag.
        /// Initialized to avoid null-reference issues.
        /// </summary>
        public Dictionary<string, int> ApplicationsByTag { get; set; }
            = new Dictionary<string, int>();
    }
}
