using System;
using System.IO;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars
{
    record ConfirmWebinarInput(Guid Id, Uri Streamyard, Uri LiveStreaming);
}
