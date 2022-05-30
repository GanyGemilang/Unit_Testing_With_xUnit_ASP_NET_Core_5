using Unit_Testing_xUnit_ASP_NET_Core.Models;
using Unit_Testing_xUnit_ASP_NET_Core.Repositories;
using Unit_Testing_xUnit_ASP_NET_Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Testing_xUnit_ASP_NET_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly UserRepository userRepository;
        private readonly LogicRepository logicRepository;
        private readonly MailRepository mailRepository;
        private readonly jwtToken jwttoken;
        private readonly PasswordRepository passwordRepository;
        private readonly ILogger Log;
        public UserController(IConfiguration configuration, ILogger<UserController> Log, UserRepository userRepository, jwtToken jwttoken, LogicRepository logicRepository, PasswordRepository passwordRepository, MailRepository mailRepository)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.jwttoken = jwttoken;
            this.logicRepository = logicRepository;
            this.passwordRepository = passwordRepository;
            this.mailRepository = mailRepository;
            this.Log = Log;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(modelLogin login)
        {
            try
            {
                if (login.username == null)
                {
                    return StatusCode(400, Utilities.Response.ResponseMessage("400", "False", "username cannot be null", login));
                }
                else if (login.password == null)
                {
                    return StatusCode(400, Utilities.Response.ResponseMessage("400", "False", "password cannot be null", login));
                }

                //Cek Logic ChangePassword
                var logic = await logicRepository.LogicLogin(login);
                if (logic.Code != "200")
                {
                    return StatusCode(int.Parse(logic.Code), Utilities.Response.ResponseMessage(logic.Code, "False", logic.Hasil, null));
                }

                //Update ke tbl_user
                var updateLogin = await userRepository.Login(logic.Hasil);
                if (updateLogin < 1)
                {
                    return StatusCode(404, Utilities.Response.ResponseMessage("404", "False", "Filed Update Login", login));
                }
                return Ok(Utilities.Response.ResponseMessage("200", "True", "Login Successful", new { Token = logic.Token }));
            }
            catch (Exception e)
            {
                return StatusCode(500, Utilities.Response.ResponseMessage("500", "False", e.Message, null));
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(modelLogout logout)
        {
            try
            {
                if (logout.username == null)
                {
                    return StatusCode(400, Utilities.Response.ResponseMessage("400", "False", "username cannot be null", logout));
                }

                var Logout = await userRepository.Logout(logout.username);
                if (Logout < 1)
                {
                    return StatusCode(404, Utilities.Response.ResponseMessage("404", "False", "Filed Update Login", Logout));
                }
                return Ok(Utilities.Response.ResponseMessage("200", "True", "Logout Successful", Logout));
            }
            catch (Exception e)
            {
                return StatusCode(500, Utilities.Response.ResponseMessage("500", "False", e.Message, null));
            }
        }   

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetUserByUsername")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            try
            {
                if (username == null)
                {
                    return StatusCode(400, Utilities.Response.ResponseMessage("400", "False", "username cannot be null", null));
                }

                var data = await userRepository.GetUserbyusername(username);
                if (data.Count < 1)
                {
                    return StatusCode(404, Utilities.Response.ResponseMessage("404", "False", "Username Not Found", null));
                }
                return Ok(Utilities.Response.ResponseMessage("200", "True", "Get data User Successful", data.FirstOrDefault()));
            }
            catch (Exception e)
            {
                return StatusCode(500, Utilities.Response.ResponseMessage("500", "False", e.Message, null));
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var data = await userRepository.GetUser();
                if (data.Count < 1)
                {
                    return StatusCode(404, Utilities.Response.ResponseMessage("404", "False", "Filed Update Login", null));
                }
                return Ok(Utilities.Response.ResponseMessage("200", "True", "Get data User Successful", data));
            }
            catch (Exception e)
            {
                return StatusCode(500, Utilities.Response.ResponseMessage("500", "False", e.Message, null));
            }
        }
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            try
            {
                if (username == null)
                {
                    return StatusCode(400, Utilities.Response.ResponseMessage("400", "False", "username cannot be null", username));
                }

                var data = await userRepository.DeleteUser(username);
                var Hasil = data.FirstOrDefault();
                if (Hasil.Code != "200")
                {
                    return StatusCode(int.Parse(Hasil.Code), Utilities.Response.ResponseMessage(Hasil.Code, "False", Hasil.Message, null));
                }
                return Ok(Utilities.Response.ResponseMessage("200", "True", Hasil.Message, username));
            }
            catch (Exception e)
            {
                return StatusCode(500, Utilities.Response.ResponseMessage("500", "False", e.Message, null));
            }
        }
    }
}
