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
    public class HomeTheatersController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        public HomeTheatersController (AppDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this._context = context;
        }

        /// <summary>
        /// Retorna uma lista com detalhes de todos os home theaters
        /// </summary>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Get api/HomeTheaters/
        /// </remarks>
        /// <returns>Lista com detalhes de todos os home theaters</returns>
        /// <response code="200">Retorna a lista requisitada</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(IEnumerable<HomeTheater>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HomeTheater>>> GetHomeTheaters()
        {
            return await _context.HomeTheaters.ToListAsync();
        }

        /// <summary>
        /// Retorna os detalhes do home theater requisitada
        /// </summary>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         GET api/HomeTheaters/abcd-12345
        /// </remarks>
        /// <param name="ID"></param>
        /// <returns>Os detalhes do home theater requisitado</returns>
        /// <response code="200">Retorna o home theater requisitado</response>
        /// <response code="400">Se não houver home theater com esse ID</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
		/// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(HomeTheater), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("{ID}")]
        public async Task<ActionResult<HomeTheater>> GetHomeTheater(string ID)
        {
            var homeTheater = await _context.HomeTheaters.FindAsync(ID);

            if (homeTheater == null)
            {
                return NotFound();
            }

            return Ok(homeTheater);
        }

		/// <summary>
    	/// Cria umo novo home theater
        /// </summary>
        /// <param name="homeTheater"></param>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Post api/HomeTheaters/
        ///         
        ///         No "body", insira os campos ID, Brand, Model, CreationDate, Value, isBeingSold, readsBlueRay
        ///         
        ///         Campo "ID" precisa ter 10 caracteres
        /// </remarks>
        /// <returns>Os detalhes do home theater criado</returns>
        /// <response code="201">Retorna os detalhes do home theater criado</response>
        /// <response code="400">Se algum dos campos do "body" da requisicao possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="409">Se ja houver um home theater com o ID fornecido</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(HomeTheater), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<ActionResult<HomeTheater>> PostHomeTheater(HomeTheater homeTheater)
        {
            _context.HomeTheaters.Add(homeTheater);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IDNotAvailable(homeTheater.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHomeTheater", new { id = homeTheater.ID }, homeTheater);
        }

		/// <summary>
        /// Altera informacoes de um home theater existente
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="homeTheater"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Put api/HomeTheater/abcd-12345
        ///         
        ///         No "body", insira os campos ID, Brand, Model, CreationDate, Value, isBeingSold, readsBlueRay
        /// </remarks>
        /// <response code="204">Atualiza o home theater requisitado</response>
        /// <response code="400">Se nao existe home theater com ID informada ou "body" possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpPut("{ID}")]
        public async Task<IActionResult> PutHomeTheater(string ID, HomeTheater homeTheater)
        {
            if (ID != homeTheater.ID)
            {
                return BadRequest();
            }

            _context.Entry(homeTheater).State = EntityState.Modified;

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
        /// Exclui um home theater existente
        /// </summary>
        /// <param name="ID"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Delete api/HomeTheaters/abcd-12345
        /// </remarks>
        /// <returns>Retorna os detalhes do home theater excluida</returns>
        /// <response code="204">Exclui um home theater existente</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="404">Se nao existe home theater com ID informado</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteHomeTheater(string ID)
        {
            var homeTheater = await _context.HomeTheaters.FindAsync(ID);
            if (homeTheater == null)
            {
                return NotFound();
            }

            _context.HomeTheaters.Remove(homeTheater);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [NonAction]
        public bool IDNotAvailable(string ID)
        {
            return _context.HomeTheaters.Any(e => e.ID == ID);
        }
    }
}
