﻿namespace Identity.Application.DTOs.Identity
{
    public class AuthResponseDto
    { 
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}