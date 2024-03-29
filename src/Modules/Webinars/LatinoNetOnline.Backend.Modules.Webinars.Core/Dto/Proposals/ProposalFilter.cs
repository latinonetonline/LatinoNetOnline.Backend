﻿using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals
{
    record ProposalFilter
    {
        public string? Title { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsActive { get; set; }
        public bool? Oldest { get; set; }
    }
}
