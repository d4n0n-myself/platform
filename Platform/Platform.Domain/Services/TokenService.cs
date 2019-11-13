﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Platform.Domain.Common;
using Platform.Fatabase;
using Platform.Fodels.Models;

namespace Platform.Domain.Services
{
	/// <summary>
	/// Service that generate JWT tokens.
	/// </summary>
	public class TokenService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public TokenService(IRepository<User> userRepository, IRepository<UserRole> userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

		internal JwtSecurityToken GenerateToken(User user)
		{
			var key = JwtOptions.GetSymmetricSecurityKey();
			var now = DateTime.Now;
			var jwt = new JwtSecurityToken(
				JwtOptions.Issuer,
				JwtOptions.Audience,
				GetIdentity(user).Claims,
				now,
				now.AddMinutes(JwtOptions.Lifetime),
				new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
			return jwt;
		}

		private ClaimsIdentity GetIdentity(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Login),
				new Claim(ClaimTypes.Email, user.Email)
			};

            var userEntity = _userRepository.FindByPredicate(x => x.Login == user.Login);

            var claimsWithRoles = _userRoleRepository
                .FindAllByPredicate(x => x.User.Id == userEntity.Id)
                .Select(userRole => new Claim(ClaimTypes.Role, userRole.Role.RoleName))
                .ToList();

            claims.AddRange(claimsWithRoles);

            var claimsIdentity =
				new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
					ClaimsIdentity.DefaultRoleClaimType);
			return claimsIdentity;
		}
	}
}