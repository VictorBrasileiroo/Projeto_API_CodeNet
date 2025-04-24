using System.ComponentModel.DataAnnotations;
using CodeNet.Application.Dto.AuthUser;
using CodeNet.Application.Dto.User;
using CodeNet.Application.Interfaces.User;
using CodeNet.Core.Models;
using CodeNet.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeNet.Api.Controllers.v1
{
    /// <summary>
    /// Controlador responsável pelas operações de gerenciamento de usuários.
    /// </summary>
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Busca um usuário pelo nome.
        /// </summary>
        /// <param name="nome">Nome do usuário a ser buscado.</param>
        /// <returns>Informações do usuário.</returns>
        [HttpGet("{nome}")]
        public async Task<IActionResult> BuscarPorNome(string nome)
        {
            try
            {
                var response = await _service.BuscarPorNome(nome);
                return Ok(new ResponseModel<UserDto>(true, "Usuário listado", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno ao buscar usuário", ex.Message));
            }
        }

        /// <summary>
        /// Remove a conta do usuário logado, mediante verificação de senha - Authorize.
        /// </summary>
        [Authorize]
        [HttpDelete("me")]
        public async Task<IActionResult> RemoverUsuario([FromBody] DeleteUserDto dto)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _service.RemoverUser(dto.Password, idUser);
                return Ok(new ResponseModel<UserModel>(true, "Usuário removido", response));
            }
            catch (UnauthorizedAccessException uaex)
            {
                return Unauthorized(new ResponseModel<string>(false, "Senha inválida", uaex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Altera a senha do usuário logado - Authorize.
        /// </summary>
        [Authorize]
        [HttpPut("alterar-senha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDto dto)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _service.AlterarSenha(dto.Password, dto.NewPassword, idUser);
                return Ok(new ResponseModel<UserModel>(true, "Senha alterada!", response));
            }
            catch (UnauthorizedAccessException uaex)
            {
                return Unauthorized(new ResponseModel<string>(false, "Senha incorreta", uaex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Atualiza o perfil do usuário logado - Authorize.
        /// </summary>
        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> EditarUser([FromBody] UserDto dto)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _service.EditarUser(dto, idUser);
                return Ok(new ResponseModel<UserModel>(true, "Usuário editado!", response));
            }
            catch (ValidationException vex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro de validação", vex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Remove um usuário específico (somente Admin) - Authorize.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("adm/{id}")]
        public async Task<IActionResult> RemoverUsuarioAdm(Guid id)
        {
            try
            {
                var response = await _service.RemoverUserAdm(id);
                return Ok(new ResponseModel<UserModel>(true, "Usuário removido com sucesso", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno ao remover usuário", ex.Message));
            }
        }

        /// <summary>
        /// Lista todos os usuários cadastrados (somente Admin) - Authorize.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var response = await _service.ListarTodos();
                return Ok(new ResponseModel<List<UserModel>>(true, "Usuários listados", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }
    }
}

