using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Resumai.DTOs;

namespace Resumai.Controllers
{
    public class ResumaiController : ControllerBase
    {
        public UserDTO? CurrentUser => HttpContext.Items["User"] as UserDTO;
    }
}