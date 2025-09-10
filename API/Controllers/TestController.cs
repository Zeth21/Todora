﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Index() 
        {
            throw new Exception("Exception test");
        }

        [HttpGet("/Test")]
        public IActionResult Test()
        {
            return Ok("It works!");
        }

        [HttpGet("/Exception")]
        public IActionResult Test2() 
        {
            throw new Exception("Ex works!");
        }
    }
}
