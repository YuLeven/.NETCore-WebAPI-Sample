using Microsoft.AspNetCore.Mvc;
using HaruGaKita.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using HaruGaKita.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using HaruGaKita.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using HaruGaKita.Services;

namespace HaruGaKita.Test.Support
{
    public class IntegrationTest : IDisposable
    {
        protected HaruGaKitaContext DbContext { get; }
        protected IDbContextTransaction Transaction { get; }
        protected IUserRepository UserRepository { get; }
        protected IUserService UserService { get; }
        private readonly IWebHost _testHost;

        public IntegrationTest()
        {
            _testHost = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseContentRoot(Configuration.ProjectRoot)
                .UseConfiguration(Configuration.BuildConfiguration())
                .UseStartup<Startup>()
                .Build();

            DbContext = _testHost.Services.GetService(typeof(HaruGaKitaContext)) as HaruGaKitaContext;
            UserRepository = new UserRepository(DbContext);
            UserService = new UserService(UserRepository);

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
