using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiHelper.Models;

namespace WebApiHelper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyAuthenticateController : ControllerBase
    {
        private IConfiguration _configuration;

        public MyAuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public Rr<UserToken> Login([FromBody]LoginUser lur)
        {
            Rr<UserToken> rr = new Rr<UserToken>();
            rr.Code = 1;
            rr.Msg = "登录成功";

            try
            {
                //验证用户名密码
                if(lur.UserPwd != "jzh")
                {
                    throw new Exception("用户名或密码错误");
                }

                //获取jwt token配置
                var tokenConfig = _configuration.GetSection("JwtToken").Get<JwtToken>();

                //创建Claim 标准的几个声明
                //iss(issuer)：签发人
                //sub(subject)：主题
                //aud(audience)：受众  接受者的Url     
                //exp(expiration time)：过期时间    这个过期时间必须要大于签发时间
                //nbf(Not Before)：生效时间   定义在什么时间之前，该jwt都是不可用的.
                //iat(Issued At)：签发时间
                //jti(JWT ID)：编号    jwt的唯一身份标识，主要用来作为一次性token,从而回避重放攻击。
                var authClaim = new[] { 
                    new Claim(JwtRegisteredClaimNames.Sub, lur.UserCode),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                IdentityModelEventSource.ShowPII = true;
                //签名密钥
                var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Secretkey));

                var token = new JwtSecurityToken(
                    issuer: tokenConfig.Issuer,//签发者
                    audience: tokenConfig.Audience,//受众 接受者的Url
                    expires: DateTime.Now.AddSeconds(tokenConfig.ValidTimeSpan),//过期时间
                    claims: authClaim,  //标准声明
                    signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)//加密
                    );

                UserToken urToken = new UserToken() {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo.ToLocalTime()
                };

                rr.Data = urToken;
                return rr;
            }
            catch(Exception ex)
            {
                rr.Code = 0;
                rr.Msg = ex.Message;
            }

            return rr;
        }
    }
}

