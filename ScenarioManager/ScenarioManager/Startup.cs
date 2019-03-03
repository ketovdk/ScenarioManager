using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ScenarioManager.Mappers;
using ScenarioManager.Mappers.AdminMapper;
using ScenarioManager.Mappers.ScenarioMapper;
using ScenarioManager.Mappers.User;
using ScenarioManager.Mappers.UserGroupMappers;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using ScenarioManager.Model.DTO;
using ScenarioManager.Model.DTO.AdminDTO;
using ScenarioManager.Model.DTO.UserGroupDTO;
using ScenarioManager.Repositories;
using ScenarioManager.Services;

namespace ScenarioManager
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
            //MsSql
            // services.AddDbContext<MainDbContext>(options =>
            // options.UseSqlServer(Configuration["MainConnectionString"]));
            // services.AddDbContext<UserDbContext>(options =>
            // options.UseSqlServer(Configuration["AccountConnectionString"]));

            //PGSql
            var str = Configuration["MainConnectionString"];
            services
              .AddEntityFrameworkNpgsql()
              .AddDbContext<MainDbContext>(options =>
                  options.UseNpgsql(str));
           /* services
              .AddEntityFrameworkNpgsql()
              .AddDbContext<UserDbContext>(options =>
                  options.UseNpgsql(Configuration["AccountConnectionString"]));*/

            services.AddScoped<IMapper<Admin, AdminWithPassword>, AdminWithPasswordMapper>();
            services.AddScoped<IMapper<ScenarioDTO, Scenario>, ScenarioMapper>();
            services.AddScoped<IMapper<UserDTO, User>, UserMapper>();

            services.AddScoped<IMapper< UserGroup, CreateUserGroup>, CreateUserGroupMapper>();
            services.AddScoped<IMapper<UserGroup, EditUserGroup>, EditUserGroupMapper>();
            services.AddScoped<UserGroupRepository>();
            services.AddScoped<ScenarioRepository>();
            services.AddScoped<TokenRepository>();
            services.AddScoped<AdminRepository>();
            services.AddScoped<UserLoginInfoRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<AccountService>();
            services.AddScoped<TokenService>();
            services.AddScoped<LoginService>();
            services.AddScoped<SensorRepository>();
            services.AddScoped<SmartThingRepository>();
            services.AddScoped<ControllerRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // укзывает, будет ли валидироваться издатель при валидации токена
                            ValidateIssuer = true,
                            // строка, представляющая издателя
                            ValidIssuer = "defaultIssuer",

                            // будет ли валидироваться потребитель токена
                            ValidateAudience = true,
                            // установка потребителя токена
                            ValidAudience = "defauleAudience",
                            // будет ли валидироваться время существования
                            ValidateLifetime = true,

                            // установка ключа безопасности
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("mysupersecret_secretkey!123")),
                            // валидация ключа безопасности
                            ValidateIssuerSigningKey = true,
                        };
                    });



            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
