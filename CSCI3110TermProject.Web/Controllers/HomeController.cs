using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CSCI3110TermProject.Data;
using CSCI3110TermProject.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace CSCI3110TermProject.Web.Controllers
{
    /// <summary>
    /// Handles public-facing pages: the dashboard, privacy notice, and error view.
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _ctx;

        // Inject logger for diagnostics and EF Core context for data access.
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        /// <summary>
        /// GET: "/"  
        /// Builds and returns the dashboard view model with summary stats.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Initialize a fresh view model
            var vm = new DashboardViewModel();

            // 1) Total number of job applications in the database
            vm.TotalApplications = await _ctx.JobApplications.CountAsync();

            // 2) Compute count of applications grouped by each tag
            var counts = await _ctx.Tags
                               .Select(tag => new {
                                   tag.Name,
                                   Count = tag.JobApplicationTags.Count()
                               })
                               .ToListAsync();
            // Populate the dictionary on the view model
            foreach (var c in counts)
                vm.ApplicationsByTag[c.Name] = c.Count;

            return View(vm);
        }

        /// <summary>
        /// GET: "/Privacy"  
        /// Displays the privacy policy page.
        /// </summary>
        public IActionResult Privacy()
            => View();

        /// <summary>
        /// Shows the error page with request ID.  
        /// Disables caching so errors always reflect the most recent failure.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}
