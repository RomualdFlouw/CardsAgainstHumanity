using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CardsAgainstHumanity.Models;
using CardsAgainstHumanity.Models.Repositories;
using Microsoft.OpenApi.Models;
using AutoMapper;
using CardsAgainstHumanity.API.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CardsAgainstHumanity.API.Hubs;
using CardsAgainstHumanity.API.Controllers;
using CardsAgainstHumanity.Models.Data;
using CardsAgainstHumanityNoSQL.Models.Repositories;
using CardsAgainstHumanityNoSQL.Models;
using static CardsAgainstHumanityNoSQL.Models.MongoSetting;
using Microsoft.Extensions.Options;

namespace CardsAgainstHumanity.API
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
            services.AddMvc(options => options.EnableEndpointRouting = false)
                 .AddNewtonsoftJson(options => {
                     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                     //circulaire referenties verhinderen
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 });

            //1.Container voor DbContext en SQLrepos
            services.AddDbContext<CardsAgainstHumanityAPIContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("CardsAgainstHumanityAPIContext")));

            //Registratie van de context NoSQL
            services.AddSingleton<CardsAgainstAPIContext>();

            //Modebinding met MongoSettings.cs:
            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));

            // Registration
            services.AddSingleton<IMongoSettings>(sp => sp.GetRequiredService<IOptions<MongoSettings>>().Value);

            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            services.AddScoped(typeof(IDeckRepo), typeof(DeckRepo));
            services.AddScoped(typeof(IGameRepo), typeof(GameRepo));

            services.AddScoped(typeof(IUserController), typeof(UserController));
            services.AddScoped(typeof(IGameController), typeof(GameController));




            //2. Services toevoegen
            //2.1 Controllers
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            }).AddNewtonsoftJson();

            //2.2 Cors
            //services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("MyAllowOrigins", builder =>
                {
                    builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    //.AllowAnyOrigin()  //vervangen door concrete Origins
                    .WithOrigins("https://localhost:44341", "http://localhost:1337", "http://192.168.0.5/:8080", "http://localhost:8080", "http://192.168.178.31:1337", "http://94.111.241.205:8080", "https://94.111.241.205:8080")
                    .AllowCredentials()  // NIET VERGETEN !
                    ;
                });

            });

            //11. Authenticatie
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<CardsAgainstHumanityAPIContext>();

            //2.3 Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CardsAgainstHumanity_API", Version = "v1" });
            });

            //2.4 Automapper volgens configuratie
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CAHProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //2.5 Rate limiting

            //12B. JWT Authenticatie
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                options.Audience = Configuration.GetSection("Tokens:Audience").Value;
                options.ClaimsIssuer = Configuration.GetSection("Tokens:Issuer").Value;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                };
                options.SaveToken = false;
                // options.RequireHttpsMetadata = false;
            });

            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CardsAgainstHumanityAPIContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //2.2 Cors
            app.UseCors("MyAllowOrigins");

            //vóór de endpoints!
            app.UseAuthentication();
            app.UseAuthorization();

            //2.3 Swagger
            app.UseSwagger(); //enable swagger
            app.UseSwaggerUI(c =>
            {
                // c.RoutePrefix = "swagger"; //path naar de UI pagina: /swagger/index.html
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CardsAgainstHumanity_API v1");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<LobbyHub>("/lobbyhub");
                endpoints.MapHub<GameHub>("/gamehub");
            });           
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //SEEDER met controler over reeds bestaande data
            //context via property dependancy
            ModelBuilderExtensions._configuration = Configuration;
            ModelBuilderExtensions._context = context;
            ModelBuilderExtensions._contentRootPath = env.ContentRootPath.Replace("CardsAgainstHumanity.API", "CardsAgainstHumanity.Models");

            ModelBuilderExtensions.SeedFromFile(); //Pas hier Hostenv beschikbaar (na Context building)
        }
    }
}
