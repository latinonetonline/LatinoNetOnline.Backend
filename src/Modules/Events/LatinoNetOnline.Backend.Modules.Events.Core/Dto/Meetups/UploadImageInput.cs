using System.IO;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups
{
    record UploadImageInput(string PhotoType, string FileName, string ContentType, Stream Stream);
}
