using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Word4Everyone.Data;
using Word4Everyone.Services;
using Word4Everyone.Services.Interfaces;

namespace Word4Everyone
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
            //Enable CORS
            services.AddCors(c =>
            {
                c.AddPolicy("MyPolocy", options => options.AllowAnyOrigin().AllowAnyMethod()
                .AllowAnyHeader());
            });

            //Dependency injection for dbContext
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //https://metanit.com/sharp/aspnet5/23.7.php
            //����������� � ������� JWT-�������
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                //RequireHttpsMetadata: ���� ����� false, �� SSL ��� �������� ������ �� ������������.
                //������ ������ ������� ���������� ������ �� ������������.� �������� ���������� ��� �� ����� 
                //������������ �������� ������ �� ��������� https.

                //TokenValidationParameters: ��������� ��������� ������ - ������� ������, ������������, 
                //    ��� ����� ����� ��������������. ���� ������ � ���� ������� ����� ��������� �������, 
                //    ������� ��������� ��������� ��������� ������� ��������� ������. �� �������� ������ 
                //    ��������: IssuerSigningKey - ���� ������������, ������� ������������� �����, � 
                //    ValidateIssuerSigningKey -���� �� ������������ ���� ������������. �� � ����� ����, 
                //    ����� ���������� ��� ������ �������, ����� ��� ����� �� ������������ �������� � 
                //    ����������� ������, ���� ����� ������, ����� ���������� �������� claims ��� ����� 
                //    � ������� ������������ � �.�.
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    // ��������, ����� �� �������������� �������� ��� ��������� ������
                    ValidateIssuer = true,
                    // ����� �� �������������� ����������� ������
                    ValidateAudience = true,
                    // ��������� ����������� ������
                    ValidAudience = Configuration["AuthSettings:Audience"],
                    // ������, �������������� ��������
                    ValidIssuer = Configuration["AuthSettings:Issuer"],
                    RequireExpirationTime = true,
                    // ��������� ����� ������������
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"])),
                    // ��������� ����� ������������
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IMailService, MailService>();

            //Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Swagger API",
                        Description = "Word4Everyone API",
                        Version = "v1"
                    });
            });

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            //Enable CORS
            app.UseCors("MyPolocy");

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger API");
            });

            //Identity
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
