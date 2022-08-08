using Application.DataTransferObjects;
using Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IMeetingService
    {
        Task<SuccessResponse<MeetingDTO>> CreateMeeting(MeetingCreateDTO model);
        Task<PagedResponse<IEnumerable<ParticipantGetDTO>>> GetParticipantsByMeeting(Guid meetingId, ResourceParameter parameter, string name, IUrlHelper urlHelper);
    }
}
