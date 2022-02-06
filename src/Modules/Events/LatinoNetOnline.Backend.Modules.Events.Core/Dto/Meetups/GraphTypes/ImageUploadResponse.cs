using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups.Objects;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups.GraphTypes
{
    internal record ImageUploadResponse(Uri UploadUrl, string ImagePath, Image Image);
}
