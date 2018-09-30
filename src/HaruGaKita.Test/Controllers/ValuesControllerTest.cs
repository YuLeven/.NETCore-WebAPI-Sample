using System;
using Xunit;
using HaruGaKita.Controllers;

namespace HaruGaKita.Test.Controllers
{
    public class ValuesControllerTest
    {
        private ValuesController _controller;
        
        public ValuesControllerTest()
        {
            _controller = new ValuesController();
        }

        [Fact]
        public async void Get_Returns_a_List_Of_Values()
        {
            var result = await _controller.Get();

            Assert.Equal
                (new string[] { "value1", "value2" },
                result.Value);
        }
    }
}
