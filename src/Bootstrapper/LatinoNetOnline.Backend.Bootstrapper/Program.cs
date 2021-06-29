using LatinoNetOnline.Backend.Bootstrapper;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Api;
using LatinoNetOnline.Backend.Modules.Events.Api;
using LatinoNetOnline.Backend.Modules.Identities.Web;
using LatinoNetOnline.Backend.Modules.Links.Api;
using LatinoNetOnline.Backend.Modules.Notifications.Api;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;

await Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build()
    .InitIdentityModule()
    .InitCallForSpeakersModule()
    .InitEventsModule()
    .InitLinksModule()
    .InitNotificationModule()
    .RunAsync();