using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApi.AuthHelper.JWT
{
    public class JWTHelper
    {
        public static string IssueJwt(TokenModelJwt tokenModelJwt) {
            string iss = AppSetting.app(new string[] { "Audience", "Issuer" });
            string audience = AppSetting.app(new string[] { "Audience", "Audience" });
            string Secret = AppSetting.app(new string[] { "Audience", "Secret" });

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,tokenModelJwt.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //这个就是过期时间，目前是过期1000秒，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,audience)
            };

            //用户权限 一个用户可多个权限
            claims.AddRange(tokenModelJwt.Role.Split(',').Select(o => new Claim(ClaimTypes.Role, o)));
            //密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            //加密
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
               issuer: iss,
               claims: claims,
               signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;

        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            long uid=0;
            long.TryParse(jwtToken.Id, out uid);
            var tm = new TokenModelJwt
            {
                Uid = uid,
                Role = role != null ? role.ToString(): "",
            };
            return tm;
        }

        /// <summary>
        /// 令牌
        /// </summary>
        public class TokenModelJwt
        {
            /// <summary>
            /// Id
            /// </summary>
            public long Uid { get; set; }
            /// <summary>
            /// 角色
            /// </summary>
            public string Role { get; set; }
            /// <summary>
            /// 职能
            /// </summary>
            public string Work { get; set; }

        }
    }
}
