﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRHunters.Common.Entities;
using HRHunters.Common.Interfaces;
using HRHunters.Common.Requests;
using HRHunters.Common.Requests.Users;
using HRHunters.Common.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace HRHunters.WebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        
        private readonly IUsersManager _usersManager;

        public AuthenticationController( IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        //[HttpPut("/")]

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterModel userRegisterModel)
        {
            if (ModelState.IsValid) {
                var result = await _usersManager.Register(userRegisterModel);
                if(result.Succeeded)
                {
                    return Ok(result);
                }
                return BadRequest(result.Errors);
            }
            return BadRequest();

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel userLoginModel)
        {
            if(ModelState.IsValid)
            {
                var user = await _usersManager.Login(userLoginModel);
                if(user.Succedeed)
                {
                    return Ok(user);
                }
                return BadRequest(user);
            }
            return BadRequest();
        }

    }
}