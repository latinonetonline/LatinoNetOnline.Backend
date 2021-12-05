using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Events
{
    record WebinarDeletedEventInput(Guid Id) : IEvent;
}
