using System.Security.Claims;

namespace HaruGaKita.Test.Support
{
    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(params Claim[] claims) : base(claims) { }
    }
}