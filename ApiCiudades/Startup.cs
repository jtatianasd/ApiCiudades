using ApiCiudades.Data;
using ApiCiudades.Helpers;
using ApiCiudades.Repository;
using ApiCiudades.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace ApiCiudades
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Conexion")));
			services.AddScoped<ICiudadRepository, CiudadRepository>();
			services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
			services.AddScoped<IUsuarioRepository, UsuarioRepository>();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Options =>
			{
				Options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
			services.AddControllers();
			services.AddAutoMapper(typeof(ApiCiudades.CiudadesMapper.CiudadesMapper));
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("ApiCiudades", new Microsoft.OpenApi.Models.OpenApiInfo()
				{
					Title = "API Ciudades",
					Version = "1",
					Description = "Backend Ciudades",
					Contact = new Microsoft.OpenApi.Models.OpenApiContact()
					{
						Email = "jtatianasd@gmail.com",
						Name = "Tatiana Salamanca",

					}
				}); ;
				var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);
				options.IncludeXmlComments(rutaApiComentarios);
			}
			);
			services.AddCors();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler(builder =>
				{
					builder.Run(async context =>
					{
						context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
						var error = context.Features.Get<IExceptionHandlerFeature>();

						if(error!=null)
						{
							context.Response.AddApplicationError(error.Error.Message);
							await context.Response.WriteAsync(error.Error.Message);
						}

					});
				});
			}
			app.UseHttpsRedirection();

			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/ApiCiudades/swagger.json", "API peliculas");
				options.RoutePrefix = "";
			});
			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
		}
	}
}
