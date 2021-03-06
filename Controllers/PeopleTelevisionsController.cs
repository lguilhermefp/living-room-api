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
    public class PeopleTelevisionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        public PeopleTelevisionsController (AppDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this._context = context;
        }

        /// <summary>
        /// Retorna uma lista com detalhes de todas as relacoes entre pessoas e televisoes
        /// </summary>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Get api/PeopleTelevisions/
        /// </remarks>
        /// <returns>Lista com detalhes de todos as relacoes entre pessoas e televisoes</returns>
        /// <response code="200">Retorna a lista requisitada</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(IEnumerable<PersonTelevision>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonTelevision>>> GetPersonTelevision()
        {
            return await _context.PeopleTelevisions.ToListAsync();
        }

        /// <summary>
        /// Retorna as relacoes entre uma pessoa e uma ou mais televisoes
        /// </summary>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         GET api/PeopleTelevisions/people/abcd-12345
        /// </remarks>
        /// <param name="personId"></param>
        /// <returns>Os detalhes da relacao requisitada</returns>
        /// <response code="200">Retorna a relacao requisitada</response>
        /// <response code="400">Se n??o houver relacao com esse ID</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
		/// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(IEnumerable<PersonTelevision>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("people/{personId}")]
        public async Task<ActionResult<IEnumerable<PersonTelevision>>> GetPersonCTelevision(string personId)
        {
            var PersonCTelevision = await _context.PeopleTelevisions.Where(pC => pC.PersonId.Equals(personId)).ToListAsync();

            if (PersonCTelevision == null)
            {
                return NotFound();
            }

            return Ok(PersonCTelevision);
        }

		/// <summary>
        /// Retorna as relacoes entre uma televisao e uma ou mais pessoas
        /// </summary>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         GET api/PeopleTelevisions/televisions/abcd-12345
        /// </remarks>
        /// <param name="televisionId"></param>
        /// <returns>Os detalhes da relacao requisitada</returns>
        /// <response code="200">Retorna a relacao requisitada</response>
        /// <response code="400">Se n??o houver relacao com esse ID</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
		/// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(IEnumerable<PersonTelevision>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("televisions/{televisionId}")]
        public async Task<ActionResult<IEnumerable<PersonTelevision>>> GetPeopleTelevision(string televisionId)
        {
            var PeopleTelevision = await _context.PeopleTelevisions.Where(pC => pC.TelevisionId.Equals(televisionId)).ToListAsync();

            if (PeopleTelevision == null)
            {
                return NotFound();
            }

            return Ok(PeopleTelevision);
        }

        /// <summary>
        /// Altera informacoes de uma relacao existente
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="personTelevision"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Put api/PeopleTelevisions/abcd-12345
        ///         
        ///         No "body", insira sua ID e a atualizacao dos campos PersonId e TelevisionId
        /// </remarks>
        /// <response code="204">Atualiza a relacao requisitada</response>
        /// <response code="400">Se nao existe relacao com ID informado ou "body" possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpPut("{ID}")]
        public async Task<IActionResult> PutPersonTelevision(string ID, PersonTelevision personTelevision)
        {
            if (ID != personTelevision.ID)
            {
                return BadRequest();
            }

            _context.Entry(personTelevision).State = EntityState.Modified;

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
        /// Cria uma nova relacao entre pessoa e televisao
        /// </summary>
        /// <param name="personTelevision"></param>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Post api/PeopleTelevisions/
        ///         
        ///         No "body", insira os campos ID, PersonId e TelevisionId
        ///         
        ///         Campo "ID" precisa ter 10 caracteres
        /// </remarks>
        /// <returns>Os detalhes da relacao criada</returns>
        /// <response code="201">Retorna os detalhes da relacao criada</response>
        /// <response code="400">Se algum dos campos do "body" da requisicao possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="409">Se ja houver uma relacao com ID fornecido</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(PersonTelevision), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<ActionResult<PersonTelevision>> PostPersonTelevision(PersonTelevision personTelevision)
        {
            _context.PeopleTelevisions.Add(personTelevision);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IDNotAvailable(personTelevision.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPersonTelevision", new { id = personTelevision.ID }, personTelevision);
        }

        /// <summary>
        /// Exclui uma relacao existente
        /// </summary>
        /// <param name="ID"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Delete api/PeopleTelevisions/abcd-12345
        /// </remarks>
        /// <returns>Retorna os detalhes da relacao excluida</returns>
        /// <response code="204">Exclui uma relacao existente</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="404">Se nao existe uma relacao com ID informado</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeletePersonTelevision(string ID)
        {
            var personTelevision = await _context.PeopleTelevisions.FindAsync(ID);
            if (personTelevision == null)
            {
                return NotFound();
            }

            _context.PeopleTelevisions.Remove(personTelevision);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [NonAction]
        public bool IDNotAvailable(string ID)
        {
            return _context.PeopleTelevisions.Any(e => e.ID == ID);
        }
    }
}
