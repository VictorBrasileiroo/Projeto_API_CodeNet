using System.ComponentModel.DataAnnotations;
using CodeNet.Application.Dto.Grupo;
using CodeNet.Application.Dto.User;
using CodeNet.Application.Interfaces.Grupo;
using CodeNet.Core.Models;
using CodeNet.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeNet.Api.Controllers.v1
{
    [Route("api/v1/grupos")]
    [ApiController]
    public class GrupoController : ControllerBase
    {
        private readonly IGrupoMembroService _serviceMembro;
        private readonly IGrupoService _serviceGrupo;

        public GrupoController(IGrupoMembroService serviceMembro, IGrupoService serviceGrupo)
        {
            _serviceMembro = serviceMembro;
            _serviceGrupo = serviceGrupo;
        }

        /// <summary>
        /// Lista todos os grupos cadastrados no sistema.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> ListarTodos()
        {
            try
            {
                var response = await _serviceGrupo.ListarTodos();
                return Ok(new ResponseModel<List<GrupoModel>>(true, "Grupos listados", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Cria um novo grupo com o usuário logado como administrador.
        /// </summary>
        [Authorize]
        [HttpPost("criar-grupo")]
        public async Task<ActionResult> CriarGrupo([FromBody] GrupoDto dto)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _serviceGrupo.CriarGrupo(dto, idUser);
                return Ok(new ResponseModel<GrupoModel>(true, "Grupo criado com sucesso!", response));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro de validação", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Título existente", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Edita os dados de um grupo, desde que o usuário seja administrador.
        /// </summary>
        [Authorize]
        [HttpPut("{idGrupo}")]
        public async Task<ActionResult> EditarGrupo([FromBody] GrupoDto dto, Guid idGrupo)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _serviceGrupo.EditarGrupo(dto, idUser, idGrupo);
                return Ok(new ResponseModel<GrupoModel>(true, "Grupo editado com sucesso!", response));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ResponseModel<string>(false, "Erro de autorização", ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de busca", ex.Message));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro de validação", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Título existente", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Exclui um grupo, desde que o usuário seja administrador.
        /// </summary>
        [Authorize]
        [HttpDelete("{idGrupo}")]
        public async Task<ActionResult> ExcluirGrupo(Guid idGrupo)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _serviceGrupo.ExcluirGrupo(idGrupo, idUser);
                return Ok(new ResponseModel<GrupoModel>(true, "Grupo excluído com sucesso!", response));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de busca", ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ResponseModel<string>(false, "Erro de autorização", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Adiciona o usuário logado ao grupo informado.
        /// </summary>
        [Authorize]
        [HttpGet("{idGrupo}/entrar-grupo")]
        public async Task<ActionResult> EntrarNoGrupo(Guid idGrupo)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _serviceMembro.EntrarNoGrupo(idUser, idGrupo);
                return Ok(new ResponseModel<GrupoMembroModel>(true, "O usuário entrou no grupo!", response));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de busca", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro de operação", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Remove o usuário logado do grupo informado.
        /// </summary>
        [Authorize]
        [HttpGet("{idGrupo}/sair-grupo")]
        public async Task<ActionResult> SairDoGrupo(Guid idGrupo)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _serviceMembro.SairDoGrupo(idUser, idGrupo);
                return Ok(new ResponseModel<GrupoMembroModel>(true, "O usuário saiu do grupo!", response));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de busca", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro de operação", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Lista todos os membros de um grupo.
        /// </summary>
        [HttpGet("{idGrupo}/membros")]
        public async Task<ActionResult> ListarMembros(Guid idGrupo)
        {
            try
            {
                var response = await _serviceMembro.ListarMembros(idGrupo);
                return Ok(new ResponseModel<List<UserResponseCoreDto>>(true, "Todos os membros listados com sucesso!", response));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de busca", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Lista todos os grupos que o usuário logado participa.
        /// </summary>
        [Authorize]
        [HttpGet("meus-grupos")]
        public async Task<ActionResult> ListarMeusGrupos()
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _serviceMembro.ListarMeusGrupos(idUser);
                return Ok(new ResponseModel<List<GrupoModel>>(true, "Todos os grupos listados com sucesso!", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }
    }
}
