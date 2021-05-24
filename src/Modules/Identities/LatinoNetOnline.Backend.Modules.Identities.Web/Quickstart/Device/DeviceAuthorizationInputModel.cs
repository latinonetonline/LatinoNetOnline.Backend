// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using LatinoNetOnline.Backend.Modules.Identities.Web.Quickstart.Consent;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Quickstart.Device
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}