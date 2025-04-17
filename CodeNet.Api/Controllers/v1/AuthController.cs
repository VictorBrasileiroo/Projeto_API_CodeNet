using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using CodeNet.Application.Dto.AuthUser;
using CodeNet.Application.Interfaces.AuthUser;
using CodeNet.Core.Models;
using CodeNet.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeNet.Api.Controllers.v1
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthUserService _service;
        public AuthController(IAuthUserService service) => _service = service;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            try
            {
                var user = await _service.Register(dto);
                var token = await _service.GerarToken(user);

                var response = new
                {
                    user.Id,
                    user.Email,
                    user.Role,
                    user.Genero,
                    user.StackPrincipal,
                    user.CriadoEm,
                    Token = token
                };

                return Ok(new ResponseModel<object>(true, "Registrado com sucesso!", response));
            }
            catch (ValidationException vex)
            {
                return StatusCode(400, new ResponseModel<string>(false, "Erro de validação", vex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(404, new ResponseModel<string>(false, "Usuário já cadastrado, tente outro email", ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                var user = await _service.ValidacaoCredenciais(dto.Email, dto.Password);
                var token = await _service.GerarToken(user);

                var response = new
                {
                    user.Id,
                    user.Nome,
                    user.Email,
                    user.Role,
                    user.Genero,
                    user.StackPrincipal,
                    user.CriadoEm,
                    Token = token
                };

                return Ok(new ResponseModel<object>(true, "Logado com sucesso!", response));
            }
            catch (Exception ex)
            {
                return StatusCode(404, new ResponseModel<string>(false, "Erro no login", ex.Message));
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> InfosUser()
        {
            try
            {
                var id = User.GetUserId();
                var user = await _service.InfosUser(id);

                return Ok(new ResponseModel<UserModel>(true, "Informações do usuário", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro Interno", ex.Message));
            }
        }
    }
}
