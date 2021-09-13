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
    public class ComputersController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        public ComputersController (AppDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this._context = context;
        }

        /// <summary>
        /// Retorna uma lista com detalhes de todos os computadores
        /// </summary>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Get api/Computers/
        /// </remarks>
        /// <returns>Lista com detalhes de todos os computadores</returns>
        /// <response code="200">Retorna a lista requisitada</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(IEnumerable<Computer>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Computer>>> GetComputers()
        {
            return await _context.Computers.ToListAsync();
        }

        /// <summary>
        /// Retorna os detalhes da computador requisitada
        /// </summary>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         GET api/Computers/abcd-12345
        /// </remarks>
        /// <param name="ID"></param>
        /// <returns>Os detalhes do computador requisitado</returns>
        /// <response code="200">Retorna o computador requisitado</response>
        /// <response code="400">Se não houver computador com esse ID</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
		/// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(Computer), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("{ID}")]
        public async Task<ActionResult<Computer>> GetComputer(string ID)
        {
            var computer = await _context.Computers.FindAsync(ID);

            if (computer == null)
            {
                return NotFound();
            }

            return Ok(computer);
        }

		/// <summary>
    	/// Cria um novo computador
        /// </summary>
        /// <param name="computer"></param>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Post api/People/
        ///         
        ///         No "body", insira os campos ID, Brand, CreationDate(opcional) e isActive
        ///         
        ///         Campo "ID" precisa ter 10 caracteres
        /// </remarks>
        /// <returns>Os detalhes do computador criado</returns>
        /// <response code="201">Retorna os detalhes do computador criado</response>
        /// <response code="400">Se algum dos campos do "body" da requisicao possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="409">Se ja houver um computador com ID fornecido</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(Computer), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<ActionResult<Computer>> PostComputer(Computer computer)
        {
            _context.Computers.Add(computer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IDNotAvailable(computer.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetComputer", new { id = computer.ID }, computer);
        }

		/// <summary>
        /// Altera informacoes de um computador existente
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="computer"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Put api/Computer/abcd-12345
        ///         
        ///         No "body", insira sua ID e a atualizacao dos campos ID, Brand, CreationDate(opcional) e isActive
        /// </remarks>
        /// <response code="204">Atualiza o computador requisitado</response>
        /// <response code="400">Se nao existe computador com ID informada ou "body" possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpPut("{ID}")]
        public async Task<IActionResult> PutComputer(string ID, Computer computer)
        {
            if (ID != computer.ID)
            {
                return BadRequest();
            }

            _context.Entry(computer).State = EntityState.Modified;

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
        /// Exclui um computador existente
        /// </summary>
        /// <param name="ID"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Delete api/Computers/abcd-12345
        /// </remarks>
        /// <returns>Retorna os detalhes do computador excluida</returns>
        /// <response code="204">Exclui um computador existente</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="404">Se nao existe computador com ID informado</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteComputer(string ID)
        {
            var computer = await _context.Computers.FindAsync(ID);
            if (computer == null)
            {
                return NotFound();
            }

            _context.Computers.Remove(computer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [NonAction]
        public bool IDNotAvailable(string ID)
        {
            return _context.People.Any(e => e.ID == ID);
        }
    }
}
