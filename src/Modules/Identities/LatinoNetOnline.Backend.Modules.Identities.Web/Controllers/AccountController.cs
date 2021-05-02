using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using System.IO;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    class AccountController : BaseMvcController
    {

        private readonly IWebHostEnvironment _environment;

        public AccountController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            //var vm = await BuildLoginViewModelAsync(returnUrl);

            //if (vm.IsExternalLoginOnly)
            //{
            //    // we only have one option for logging in and it's an external provider
            //    return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
            //}

            //return View();

            var myfile = System.IO.File.ReadAllText(Path.Combine(new FileInfo(typeof(AccountController).Assembly.Location).DirectoryName ?? string.Empty, "IdentitySite/Login.html"));
            return Content(myfile, "text/html");

        }
    }
}
