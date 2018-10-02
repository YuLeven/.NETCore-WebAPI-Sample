using System;
using Xunit;
using HaruGaKita.Controllers;
using HaruGaKita.Infrastructure.Interfaces;
using Moq;
using HaruGaKita.Entities;
using System.Collections.Generic;

namespace HaruGaKita.Test.Controllers
{
    public class ValuesControllerTest
    {
        private ValuesController _controller;
        private Mock<IUserRepository> _mockRepository;

        public ValuesControllerTest()
        {
            _mockRepository = new Mock<IUserRepository>();
            _controller = new ValuesController(_mockRepository.Object);
        }

        [Fact]
        public async void Get_Returns_a_List_Of_Values()
        {
            var result = await _controller.Get();
            var mockUser = new User();

            _mockRepository.Verify(x => x.ListAllAsync(), Times.Once);
        }
    }
}
