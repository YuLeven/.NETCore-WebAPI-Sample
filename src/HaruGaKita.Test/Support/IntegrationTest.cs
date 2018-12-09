using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using HaruGaKita.Persistence;
using HaruGaKita.WebAPI;
using HaruGaKita.Domain.Entities;

namespace HaruGaKita.Test.Support
{
    public class IntegrationTest : IDisposable
    {
        protected HaruGaKitaDbContext DbContext { get; }
        protected IDbContextTransaction Transaction { get; }
        private readonly IWebHost _testHost;

        public IntegrationTest()
        {
            _testHost = new WebHostBuilder()
                .UseEnvironment("Test")
                .UseContentRoot(Configuration.ProjectRoot)
                .UseConfiguration(Configuration.BuildConfiguration())
                .UseStartup<Startup>()
                .Build();

            DbContext = _testHost.Services.GetService(typeof(HaruGaKitaDbContext)) as HaruGaKitaDbContext;

            Transaction = DbContext.Database.BeginTransaction();
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
            }
        }

        protected void UseAuthenticatedUser(ControllerBase controller, User user)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new TestPrincipal(new Claim(JwtRegisteredClaimNames.Sub, user.Uid.ToString()))
                }
            };
        }
    }
}
