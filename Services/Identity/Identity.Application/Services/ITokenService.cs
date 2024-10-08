﻿using Identity.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Services;

public interface ITokenService
{
    string CreateToken(ApplicationUser user, IEnumerable<string> roles);
}