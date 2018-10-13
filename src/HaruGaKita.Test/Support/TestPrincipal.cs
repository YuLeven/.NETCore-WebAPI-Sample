using System.Security.Claims;

namespace HaruGaKita.Test.Support
{
    public class TestPrincipal : ClaimsPrincipal
    {
        public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims)) { }
    }
}