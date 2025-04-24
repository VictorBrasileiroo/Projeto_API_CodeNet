using CodeNet.Application.Dto.Mensagem;
using CodeNet.Application.Interfaces.Mensagem;
using CodeNet.Core.Models;
using CodeNet.Core.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeNet.Api.Controllers.v1
{
    [Route("api/v1/grupos")]
    [ApiController]
    public class MensagemController : ControllerBase
    {
        private readonly IMensagemService _service;

        public MensagemController(IMensagemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Envia uma nova mensagem para um grupo específico - Authorize.
        /// </summary>
        /// <param name="idGrupo">ID do grupo onde a mensagem será enviada</param>
        /// <param name="dto">Dados da mensagem a ser enviada</param>
        /// <returns>Mensagem enviada com sucesso</returns>
        [Authorize]
        [HttpPost("{idGrupo}/mensagens/enviar-mensagem")]
        public async Task<IActionResult> EnviarMensagem(Guid idGrupo, [FromBody] MensagemDto dto)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _service.EnviarMensagem(dto, idUser, idGrupo);
                return Ok(new ResponseModel<MensagemModel>(true, "Mensagem Enviada", response));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de busca", ex.Message));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro de validação", ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Lista todas as mensagens de um grupo - Authorize.
        /// </summary>
        /// <param name="idGrupo">ID do grupo</param>
        /// <returns>Lista de mensagens do grupo</returns>
        [Authorize]
        [HttpGet("{idGrupo}/mensagens")]
        public async Task<IActionResult> ListarMensagensGrupo(Guid idGrupo)
        {
            try
            {
                var response = await _service.ListarMensagensDoGrupo(idGrupo);
                return Ok(new ResponseModel<List<MensagemListaDto>>(true, "Mensagens listadas", response));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de busca", ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Edita uma mensagem enviada anteriormente, se for do próprio autor - Authorize.
        /// </summary>
        /// <param name="idGrupo">ID do grupo onde a mensagem está</param>
        /// <param name="idMensagem">ID da mensagem a ser editada</param>
        /// <param name="dto">Novos dados da mensagem</param>
        /// <returns>Mensagem editada com sucesso</returns>
        [Authorize]
        [HttpPut("{idGrupo}/mensagens/{idMensagem}/editar-mensagem")]
        public async Task<IActionResult> EditarMensagem(Guid idGrupo, Guid idMensagem, [FromBody] MensagemDto dto)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _service.EditarMensagem(dto, idMensagem, idUser);
                return Ok(new ResponseModel<MensagemModel>(true, "Mensagem editada", response));
            }
            catch (UnauthorizedAccessException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de autorização", ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de busca", ex.Message));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro de validação", ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }

        /// <summary>
        /// Exclui uma mensagem do grupo, se o usuário for o autor ou um administrador - Authorize.
        /// </summary>
        /// <param name="idGrupo">ID do grupo onde a mensagem está</param>
        /// <param name="idMensagem">ID da mensagem a ser excluída</param>
        /// <returns>Mensagem excluída com sucesso</returns>
        [Authorize]
        [HttpDelete("{idGrupo}/mensagens/{idMensagem}/excluir-mensagem")]
        public async Task<IActionResult> ExcluirMensagem(Guid idGrupo, Guid idMensagem)
        {
            try
            {
                var idUser = User.GetUserId();
                var response = await _service.ExcluirMensagem(idMensagem, idUser);
                return Ok(new ResponseModel<MensagemModel>(true, "Mensagem excluída", response));
            }
            catch (UnauthorizedAccessException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de autorização", ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(false, "Erro de busca", ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>(false, "Erro interno", ex.Message));
            }
        }
    }
}
