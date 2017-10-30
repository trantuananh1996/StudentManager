using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using exam.Models;
using exam.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
namespace exam
{
    public class Startup
    {
        private string secrectKey = "needtogetthisfromenvironment";
        private string connectStr = "Server=localhost;database=quanlihocsinh;uid=root;pwd=root";
        public Startup(IConfiguration configuration)
        {
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });            

            services.AddDbContext<ApplicationDbContext>(options =>
                                                        options.UseMySql(connectStr));

            services.AddAuthentication(o => {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;                
            })
                    .AddJwtBearer(cfg => {
                        cfg.SaveToken = true;
                        cfg.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.secrectKey))
                        };
                    });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Exmam API", Version = "v1" });
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<INationRepository, NationRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IReligionRepository, ReligionRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IStudentClassRepository, StudentClassRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
           
            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
