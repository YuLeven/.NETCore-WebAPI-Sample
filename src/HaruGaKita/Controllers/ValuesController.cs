using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaruGaKita.Entities;
using HaruGaKita.Infrastructure.Data;
using HaruGaKita.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HaruGaKita.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public ValuesController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var newUser = new User();
            newUser.Email = "foo@bar.com";

            await _userRepository.AddAsync(newUser);
            return await _userRepository.ListAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(Guid id)
        {
            var user = await _userRepository.GetByGuidAsync(id);
            if (user == null) { return NotFound(new {
                error = $"Can't find user with id {id}"
            }); }
            return user;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
