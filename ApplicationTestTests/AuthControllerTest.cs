using System;
using Xunit;
using TestApplication.Controllers;
using TestApplication.Data;
using TestApplication.Data.Other;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationTestTests
{
    public class AuthControllerTest
    {
        private readonly ServiceCollection services; 
        private readonly AuthController authController;
        private readonly DataContext dataContext;
        private readonly IUserRepository userRepository;
        private readonly JwtService jwtService;

        public AuthControllerTest()
        {

            services = new ServiceCollection();
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("InMemDb"));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<JwtService>();

            var serviceProvider = services.BuildServiceProvider();

            this.dataContext = serviceProvider.GetService<DataContext>();
            this.userRepository = serviceProvider.GetService<IUserRepository>();
            this.jwtService = serviceProvider.GetService<JwtService>();

            this.authController = new AuthController(dataContext,userRepository,jwtService);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}
