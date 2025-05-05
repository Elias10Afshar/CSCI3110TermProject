using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCI3110TermProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CSCI3110TermProject.Data
{
    /// <summary>
    /// My application’s user class—right now it just pulls in all the built‑in IdentityUser properties
    /// (Username, Email, PasswordHash, etc.), but I can extend it later with custom fields.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {

    }
}
