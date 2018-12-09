#pragma warning disable CS1591
using System;
using Microsoft.IdentityModel.Tokens;

namespace HaruGaKita.Common
{
    public static class Configuration
    {
        public static string ApplicationSecret = Environment.GetEnvironmentVariable("JWT_SIGNING_SECRET") ?? "izKqpWVZ2KZhh5&YeHC&9fAvZ6q$@psO8aCTKdN8h2aV3MF%GrZ#4zATBOBZEA29UkWPIpwZ6pJa0%HGeM5ZSH$ltmD!9@MZdOnw";
        public static SecurityKey ApplicationSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(ApplicationSecret));
        public static string ApiAudience = "api.harugakita.com.br";
        public static string AppAuthority = "harugakita";
        public static int BCryptWorkFactor = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test" ? 4 : 10;
    }
}