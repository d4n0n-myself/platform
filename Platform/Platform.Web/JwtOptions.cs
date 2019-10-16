﻿using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Platform.Web
{
	public static class JwtOptions
	{
		public const string Issuer = "InfoSystemAuthServer";

		public const string Audience = "http://localhost:5000/";

		public const string Key = "mysupersecret_secretkey!123";
		
		public const int Lifetime = 120;
        
		public static SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
		}
	}
}