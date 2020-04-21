using System;
using System.Collections.Generic;
using System.Text;

namespace UsefulServices.Dtos.Users
{
    public class RegisterUserDto : AuthUserDto
    {
        public string Displayname { get; set; }
    }
}
