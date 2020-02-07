using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JustChat
{
    public static class AuthOptions
    {
        public const string ISSUER = "Just Chat Auth Server";
        public const string AUDIENCE = "Just Chat Client";
        const string KEY = "j3qq4-h7h2v-2hch4-m3hk8-6m8vw";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
