using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiHelper.Models;
using WebApiHelper.Filters;

namespace WebApiHelper
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
            //配置跨域
            services.AddCors(options => options.AddPolicy("cors",
                p => p.SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            #region AspNetCoreRateLimit
            // needed to load configuration from appsettings.json
            services.AddOptions();

            //needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsetting.json
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            //inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            //inject counter and rules distributed cache stores
            //上边的2行是表示 用内存来存储计数信息，如果要用到 其它方式存储 如redis等 则用以下2行来代替
            //services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            //services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();

            //configuration (resolvers,  counter  key builder)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            #endregion

            #region 添加JWT认证授权 使用 JwtBearer 作为认证中间件
            //获取Token配置
            var tokenConfig = Configuration.GetSection("JwtToken").Get<JwtToken>();
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                //如果验证 签发者 和 受众  ValidateIssuer  ValidAudience，则必须和签发jwt的设置一样
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {
                    ValidateIssuer = tokenConfig.ValidateIssuer,//是否验证 Issuer jwt签发者
                    ValidateAudience = tokenConfig.ValidateAudience,  //是否验证Audience  jwt所面向的用户
                    ValidateLifetime = tokenConfig.ValidateLifetime,//是否验证失效时间          
                    ValidIssuer = tokenConfig.Issuer,
                    ValidAudience = tokenConfig.Audience,//这两项必须和 签发密钥的时候 设置一样
                    ClockSkew = TimeSpan.Zero,//校验时间是否过期时，设置的时钟偏移量
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Secretkey))
                };
            });
            #endregion

            services.AddControllers(options => {
                //添加全局ActionFilter
                options.Filters.Add<MyGlobalActionFilter>();

            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiHelper", Version = "v1" });
                //Swagger UI页面添加 认证权限按钮 不支持JwtBearer 方式
                //c.AddSecurityDefinition("jwtauth", new OpenApiSecurityScheme
                //{
                //    Description = "JWT授权",
                //    Name = "Authorization",//jwt的默认参数名称
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey
                //});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiHelper v1"));
            }

            //允许跨域
            app.UseCors("cors");

            #region AspNetCoreRateLimit
            app.UseIpRateLimiting();
            #endregion

            #region 认证jwt中间件
            //开启jwt认证(注意  先认证 再授权)
            app.UseAuthentication();
            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            //开启jwt授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
