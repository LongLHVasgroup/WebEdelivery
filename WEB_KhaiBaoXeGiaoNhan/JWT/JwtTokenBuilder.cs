using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;

namespace BearerHelper
{
    public sealed class JwtTokenBuilder
    {
        private SecurityKey securityKey = null;
        private string subject = "";
        private string issuer = "";
        private string audience = "";
        private Dictionary<string, string> claims = new Dictionary<string, string>();
        private int expiryInDays = 7;
        /// <summary>
        /// Key khởi tạo token
        /// </summary>
        /// <param name="securityKey">String key</param>
        /// <returns>Add key để khởi tạo token</returns>
        public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this.securityKey = securityKey;
            return this;
        }
        /// <summary>
        /// Thêm đối tượng cần tạo token
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public JwtTokenBuilder AddSubject(string subject)
        {
            this.subject = subject;
            return this;
        }
        /// <summary>
        /// Add Issuer (nếu ValidateIssuer là true)
        /// </summary>
        /// <param name="issuer"></param>
        /// <returns></returns>
        public JwtTokenBuilder AddIssuer(string issuer)
        {
            this.issuer = issuer;
            return this;
        }
        /// <summary>
        /// Add Audience (nếu ValidateAudience là true)
        /// </summary>
        /// <param name="audience"></param>
        /// <returns></returns>
        public JwtTokenBuilder AddAudience(string audience)
        {
            this.audience = audience;
            return this;
        }
        /// <summary>
        /// Add Policy
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns>Type: Policy tương ứng cần lấy, Value: giá trị</returns>
        public JwtTokenBuilder AddClaim(string type, string value)
        {
            this.claims.Add(type, value);
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
        {
            this.claims.Union(claims);
            return this;
        }
        /// <summary>
        /// Thời hạn hiệu lực của token (theo ngày)
        /// </summary>
        /// <param name="expiryInDays"></param>
        /// <returns></returns>
        public JwtTokenBuilder AddExpiry(int expiryInDays)
        {
            this.expiryInDays = expiryInDays;
            return this;
        }
        /// <summary>
        /// Khởi tạo token
        /// </summary>
        /// <returns></returns>
        public JwtToken Build()
        {
            EnsureArguments();

            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, this.subject),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(this.claims.Select(item => new Claim(item.Key, item.Value)));

            var token = new JwtSecurityToken(
                              issuer: this.issuer,
                              audience: this.audience,
                              claims: claims,
                              expires: DateTime.UtcNow.AddDays(expiryInDays),
                              signingCredentials: new SigningCredentials(
                                                        this.securityKey,
                                                        SecurityAlgorithms.HmacSha256));

            return new JwtToken(token);
        }

        #region " private "

        private void EnsureArguments()
        {
            if (this.securityKey == null)
                throw new ArgumentNullException("Security Key");

            if (string.IsNullOrEmpty(this.subject))
                throw new ArgumentNullException("Subject");

            if (string.IsNullOrEmpty(this.issuer))
                throw new ArgumentNullException("Issuer");

            if (string.IsNullOrEmpty(this.audience))
                throw new ArgumentNullException("Audience");
        }

        #endregion
    }
}
