
﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Resumai.Db;
using Resumai.DTOs;

namespace Resumai.Middlewares
{
    public class UserValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMapper _mapper;

        public UserValidationMiddleware(RequestDelegate next, IMapper mapper)
        {
            _next = next;
            _mapper = mapper;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext db)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {

                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {

                    var userDTO = new UserDTO
                    {
                        Email = context.User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                        Id = Guid.Parse(userId),
                        Location = context.User.FindFirst(ClaimTypes.Locality)?.Value ?? "",
                        Name = context.User.FindFirst(ClaimTypes.Name)?.Value ?? "",
                        Username = context.User.FindFirst(ClaimTypes.UserData)?.Value ?? ""
                    };

                    context.Items["User"] = userDTO;
                }
            }
            await _next(context);
        }
    }
}
