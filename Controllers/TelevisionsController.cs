using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using living_room_api.Data;

namespace living_room_api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TelevisionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        public TelevisionsController (AppDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this._context = context;
        }

        /// <summary>
        /// Retorna uma lista com detalhes de todas as televisoes
        /// </summary>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Get api/Televisions/
        /// </remarks>
        /// <returns>Lista com detalhes de todas as televisoes</returns>
        /// <response code="200">Retorna a lista requisitada</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se houve falha na conexao com o banco de dados</response>
        [ProducesResponseType(typeof(IEnumerable<Television>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Television>>> GetTelevisions()
        {
            return await _context.Televisions.ToListAsync();
        }

        /// <summary>
        /// Retorna os detalhes da televisao requisitada
        /// </summary>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         GET api/Televisions/abcd-12345
        /// </remarks>
        /// <param name="ID"></param>
        /// <returns>Os detalhes da televisao requisitada</returns>
        /// <response code="200">Retorna a televisao requisitada</response>
        /// <response code="400">Se não houver televisao com esse ID</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
		/// <response code="500">Se o banco de dados retournou erro</response>
        [ProducesResponseType(typeof(Television), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("{ID}")]
        public async Task<ActionResult<Television>> GetTelevision(string ID)
        {
            var television = await _context.Televisions.FindAsync(ID);

            if (television == null)
            {
                return NotFound();
            }

            return Ok(television);
        }

		/// <summary>
    	/// Cria uma nova televisao
        /// </summary>
        /// <param name="television"></param>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Post api/Televisions/
        ///         
        ///         No "body", insira os campos ID, Brand, Model, CreationDate, Value, is3D, isBeingSold
        ///         
        ///         Campo "ID" precisa ter 10 caracteres
        /// </remarks>
        /// <returns>Os detalhes da televisao criada</returns>
        /// <response code="201">Retorna os detalhes da televisao criada</response>
        /// <response code="400">Se algum dos campos do "body" da requisicao possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="409">Se ja houver uma televisao com ID fornecido</response>
        /// <response code="500">Se o banco de dados retournou erro</response>
        [ProducesResponseType(typeof(Television), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<ActionResult<Television>> PostTelevision(Television television)
        {
            _context.Televisions.Add(television);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IDNotAvailable(television.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTelevision", new { id = television.ID }, television);
        }

		/// <summary>
        /// Altera informacoes de uma televisao existente
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="television"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Put api/Television/abcd-12345
        ///         
        ///         No "body", insira os campos ID, Brand, Model, CreationDate, Value, is3D, isBeingSold
        /// </remarks>
        /// <response code="204">Atualiza a televisao requisitada</response>
        /// <response code="400">Se nao existe televisao com ID informada ou "body" possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retournou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpPut("{ID}")]
        public async Task<IActionResult> PutTelevision(string ID, Television television)
        {
            if (ID != television.ID)
            {
                return BadRequest();
            }

            _context.Entry(television).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IDNotAvailable(ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        /// <summary>
        /// Exclui uma televisao existente
        /// </summary>
        /// <param name="ID"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Delete api/Televisions/abcd-12345
        /// </remarks>
        /// <returns>Retorna os detalhes da televisao excluida</returns>
        /// <response code="204">Exclui uma televisao existente</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="404">Se nao existe televisao com ID informado</response>
        /// <response code="500">Se o banco de dados retournou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteTelevision(string ID)
        {
            var television = await _context.Televisions.FindAsync(ID);
            if (television == null)
            {
                return NotFound();
            }

            _context.Televisions.Remove(television);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [NonAction]
        public bool IDNotAvailable(string ID)
        {
            return _context.Televisions.Any(e => e.ID == ID);
        }
    }
}
