using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Resumai.DTOs
{
    [AutoMap(typeof(Models.User))]
    public class UserDTO
    {
        public required  Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; } 
        public required string Email { get; set; }

    }
}