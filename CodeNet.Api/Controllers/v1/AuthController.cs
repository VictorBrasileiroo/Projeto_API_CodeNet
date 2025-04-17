using System.ComponentModel.DataAnnotations;
using CodeNet.Application.Dto.AuthUser;
using CodeNet.Application.Interfaces.AuthUser;
using CodeNet.Core.Models;
using CodeNet.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeNet.Api.Controllers.v1
{
    /// <summary>
    /// Controlador responsável pelas operações de autenticação de usuários.
    /// </summary>
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthUserService _service;
        public AuthController(IAuthUserService service) => _service = service;

        /// <summary>
        /// Registra um novo usuário na aplicação.
        /// </summary>
        /// <param name="dto">Dados de cadastro do usuário.</param>
        /// <returns>Retorna os dados do usuário criado e um token JWT.</returns>
        /// <response code="200">Usuário registrado com sucesso.</response>
        /// <response code="400">Erro de validação.</response>
        /// <response code="404">Usuário já existe com o e-mail informado.</response>
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
                    user.Nome,
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

        /// <summary>
        /// Realiza o login do usuário e gera um token JWT.
        /// </summary>
        /// <param name="dto">Dados de login do usuário.</param>
        /// <returns>Retorna os dados do usuário autenticado e o token JWT.</returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="404">Credenciais inválidas.</response>
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

        /// <summary>
        /// Retorna as informações do usuário autenticado.
        /// </summary>
        /// <returns>Dados completos do usuário logado.</returns>
        /// <response code="200">Informações obtidas com sucesso.</response>
        /// <response code="500">Erro interno ao recuperar informações do usuário.</response>
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
