using LatinoNetOnline.Backend.Modules.Notifications.Api.Controllers;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Linq;
using System.Threading.Tasks;

using WebPushDemo.Models;

namespace WebPushDemo.Controllers
{
    class DevicesController : BaseController
    {
        private readonly NotificationDbContext _context;

        private readonly IConfiguration _configuration;

        public DevicesController(NotificationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Devices
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Devices.ToListAsync());
        }

        // GET: Devices/Create
        //public IActionResult Create()
        //{
        //    ViewBag.PublicKey = _configuration.GetSection("VapidKeys")["PublicKey"];

        //    return View();
        //}

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(Device device)
        {
            bool alreadyExist = await _context.Devices.AnyAsync(x => x.PushEndpoint == device.PushEndpoint);


            if (alreadyExist)
            {
                device = await _context.Devices.SingleAsync(x => x.PushEndpoint == device.PushEndpoint);
            }
            else
            {

                if (HttpContext.User is not null && HttpContext.User.Identity.IsAuthenticated)
                {
                    device.UserId = Guid.Parse(HttpContext.User.FindFirst("sub").Value);
                }

                _context.Add(device);
                await _context.SaveChangesAsync();
            }

            return Ok(OperationResult<Device>.Success(device));
        }

        // GET: Devices/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var devices = await _context.Devices
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (devices == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(devices);
        //}

        // POST: Devices/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var devices = await _context.Devices.SingleOrDefaultAsync(m => m.Id == id);
            _context.Devices.Remove(devices);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
