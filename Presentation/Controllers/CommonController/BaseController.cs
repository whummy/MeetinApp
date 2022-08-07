using Microsoft.AspNetCore.Mvc;
using Application.DataTransferObjects;
using API.Helpers;

namespace API.Controllers.CommonController
{
	[ApiController]
	public class BaseController : ControllerBase
	{
		public LoggedinUserDto LoggedInUser => new()
		{
			UserId = WebHelper.UserId,
			FirstName = WebHelper.FirstName,
			LastName = WebHelper.LastName,
			Email = WebHelper.Email,
			//InstanceId = WebHelper.InstanceId,
			Roles = WebHelper.Roles
		};
	}
}
