
using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Reflection;
using DiagramGenerator.Domain.Services.Interfaces;
using DiagramGenerator.Domain.Services;
using DiagramGenerator.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DiagramGenerator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DiagramGeneratorContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Token:Issuer"],
                        ValidAudience = Configuration["Token:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"])),
                    };
                });

            services.AddTransient<IMailService, NullMailService>();

            //services.AddControllers();
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();

            services.AddRazorPages();

            services.AddSwaggerGen(c =>
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    });

                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Diagram Generator",
                        Description = "Diagram Generator API",
                        TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Adam Slomian",
                            Email = string.Empty
                        }
                    });
                }
            );

            services.AddDbContext<DiagramGeneratorContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DiagramGenerator")));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddAutoMapper();

            services.AddScoped<IDiagramRepository, DiagramRepository>();
            services.AddScoped<IInputRepository, InputRepository>();
            services.AddScoped<ICriterionRepository, CriterionRepository>();
            services.AddScoped<IMethodRepository, MethodRepository>();
            services.AddScoped<IRequirementRepository, RequirementRepository>();
            services.AddScoped<IOutputRepository, OutputRepository>();
            services.AddScoped<IProcessRepository, ProcessRepository>();
            services.AddScoped<IOperationRepository, OperationRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDiagramRepository, DiagramRepository>();
            services.AddScoped<ICriterionRepository, CriterionRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();

            services.AddScoped<IDiagramManager, DiagramManager>();
            services.AddScoped<IInputManager, InputManager>();
            services.AddScoped<ICriterionManager, CriterionManager>();
            services.AddScoped<IMethodManager, MethodManager>();
            services.AddScoped<IRequirementManager, RequirementManager>();
            services.AddScoped<IOutputManager, OutputManager>();
            services.AddScoped<IProcessManager, ProcessManager>();
            services.AddScoped<IOperationManager, OperationManager>();
            services.AddScoped<ISupplierManager, SupplierManager>();
            services.AddScoped<IClientManager, ClientManager>();
            services.AddScoped<IUserSeedManager, UserSeedManager>();
            services.AddScoped<IWordExportManager, WordExportManager>();
            services.AddScoped<IPdfExportManager, PdfExportManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/error");
            //}

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Diagram Generator API V1");
            });

            CreateDatabase(app);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    //await context.Response.WriteAsync("Hello World!");
                //});

                //    endpoints.MapControllerRoute("default", "/{controller}/{action}/{id?}",
                //        new { controler = "App", action = "Index" });
                //});

                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller=app}/{action=Index}/{id?}");
            });
        }

        private void CreateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DiagramGeneratorContext>();
                context.Database.Migrate();
            }
        }
    }
}
