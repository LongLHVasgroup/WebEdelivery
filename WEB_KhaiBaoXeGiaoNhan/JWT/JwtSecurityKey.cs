using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace BearerHelper
{
    /// <summary>
    /// 
    /// </summary>
    public static class JwtSecurityKey
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}
