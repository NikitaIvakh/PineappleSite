﻿namespace Identity.Domain.DTOs.Authentications
{
    public interface IRegisterRequestDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}