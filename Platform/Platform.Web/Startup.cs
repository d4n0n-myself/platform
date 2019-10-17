using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VueCliMiddleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Platform.Models;

namespace Platform.Web
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			AddJwtAuthentication(services);

			services.AddSpaStaticFiles(opt => opt.RootPath = "ClientApp/dist");

			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo()
				{
					Title = "Platform Swagger API",
					Version = "v1"
				});
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			ApplicationConfiguration.Initialize();

			app.UseAuthentication();

			app.UseSwagger();
			app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Platform API"); });

            app.UseSpaStaticFiles();

            app.UseRouting();
			
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();                

                endpoints.MapToVueCliProxy(
                    "{*path}",
                    new SpaOptions { SourcePath = "ClientApp" },
                    npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null
                    );
            });
		}

		public void AddJwtAuthentication(IServiceCollection services)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = "BarsPlatform",
					ValidateAudience = true,
					ValidAudience = "People",
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("MySuperSecretKey")),
					ValidateIssuerSigningKey = true,
				};
			});
		}
	}
}