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
            //���ÿ���
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
            //�ϱߵ�2���Ǳ�ʾ ���ڴ����洢������Ϣ�����Ҫ�õ� ������ʽ�洢 ��redis�� ��������2��������
            //services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            //services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();

            //configuration (resolvers,  counter  key builder)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            #endregion

            #region ���JWT��֤��Ȩ ʹ�� JwtBearer ��Ϊ��֤�м��
            //��ȡToken����
            var tokenConfig = Configuration.GetSection("JwtToken").Get<JwtToken>();
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                //�����֤ ǩ���� �� ����  ValidateIssuer  ValidAudience��������ǩ��jwt������һ��
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {
                    ValidateIssuer = tokenConfig.ValidateIssuer,//�Ƿ���֤ Issuer jwtǩ����
                    ValidateAudience = tokenConfig.ValidateAudience,  //�Ƿ���֤Audience  jwt��������û�
                    ValidateLifetime = tokenConfig.ValidateLifetime,//�Ƿ���֤ʧЧʱ��          
                    ValidIssuer = tokenConfig.Issuer,
                    ValidAudience = tokenConfig.Audience,//���������� ǩ����Կ��ʱ�� ����һ��
                    ClockSkew = TimeSpan.Zero,//У��ʱ���Ƿ����ʱ�����õ�ʱ��ƫ����
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Secretkey))
                };
            });
            #endregion

            services.AddControllers(options => {
                //���ȫ��ActionFilter
                options.Filters.Add<MyGlobalActionFilter>();

            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiHelper", Version = "v1" });
                //Swagger UIҳ����� ��֤Ȩ�ް�ť ��֧��JwtBearer ��ʽ
                //c.AddSecurityDefinition("jwtauth", new OpenApiSecurityScheme
                //{
                //    Description = "JWT��Ȩ",
                //    Name = "Authorization",//jwt��Ĭ�ϲ�������
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

            //�������
            app.UseCors("cors");

            #region AspNetCoreRateLimit
            app.UseIpRateLimiting();
            #endregion

            #region ��֤jwt�м��
            //����jwt��֤(ע��  ����֤ ����Ȩ)
            app.UseAuthentication();
            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            //����jwt��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
