using Application.DataTransferObjects;
using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IParticipantService
    {
        Task<SuccessResponse<string>> JoinMeeting(int meetingCode, LoggedinUserDto loggedinUser);
    }
}
