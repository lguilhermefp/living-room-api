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
    public class PeopleComputersController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        public PeopleComputersController (AppDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this._context = context;
        }

        /// <summary>
        /// Retorna uma lista com detalhes de todos as relacoes entre pessoas e computadores
        /// </summary>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Get api/PeopleComputers/
        /// </remarks>
        /// <returns>Lista com detalhes de todos as relacoes entre pessoas e computadores</returns>
        /// <response code="200">Retorna a lista requisitada</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(IEnumerable<PersonComputer>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonComputer>>> GetPersonComputer()
        {
            return await _context.PeopleComputers.ToListAsync();
        }

        /// <summary>
        /// Retorna as relacoes entre uma pessoa e um ou mais computadores
        /// </summary>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         GET api/PeopleComputers/people/abcd-12345
        /// </remarks>
        /// <param name="personId"></param>
        /// <returns>Os detalhes da relacao requisitada</returns>
        /// <response code="200">Retorna a relacao requisitada</response>
        /// <response code="400">Se não houver relacao com esse ID</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
		/// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(IEnumerable<PersonComputer>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("people/{personId}")]
        public async Task<ActionResult<IEnumerable<PersonComputer>>> GetPersonComputers(string personId)
        {
            var PersonComputers = await _context.PeopleComputers.Where(pC => pC.PersonId.Equals(personId)).ToListAsync();

            if (PersonComputers == null)
            {
                return NotFound();
            }

            return Ok(PersonComputers);
        }

		/// <summary>
        /// Retorna as relacoes entre um computador e uma ou mais pessoas
        /// </summary>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         GET api/PeopleComputers/computers/abcd-12345
        /// </remarks>
        /// <param name="computerId"></param>
        /// <returns>Os detalhes da relacao requisitada</returns>
        /// <response code="200">Retorna a relacao requisitada</response>
        /// <response code="400">Se não houver relacao com esse ID</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
		/// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(IEnumerable<PersonComputer>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("computers/{computerId}")]
        public async Task<ActionResult<IEnumerable<PersonComputer>>> GetPeopleComputer(string computerId)
        {
            var PeopleComputer = await _context.PeopleComputers.Where(pC => pC.ComputerId.Equals(computerId)).ToListAsync();

            if (PeopleComputer == null)
            {
                return NotFound();
            }

            return Ok(PeopleComputer);
        }

        /// <summary>
        /// Altera informacoes de uma relacao existente
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="personComputer"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Put api/PeopleComputers/abcd-12345
        ///         
        ///         No "body", insira sua ID e a atualizacao dos campos PersonId e ComputerId
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
        public async Task<IActionResult> PutPersonComputer(string ID, PersonComputer personComputer)
        {
            if (ID != personComputer.ID)
            {
                return BadRequest();
            }

            _context.Entry(personComputer).State = EntityState.Modified;

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
        /// Cria uma nova relacao entre pessoa e computador
        /// </summary>
        /// <param name="personComputer"></param>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Post api/PeopleComputers/
        ///         
        ///         No "body", insira os campos ID, PersonId e ComputerId
        ///         
        ///         Campo "ID" precisa ter 10 caracteres
        /// </remarks>
        /// <returns>Os detalhes da relacao criada</returns>
        /// <response code="201">Retorna os detalhes da relacao criada</response>
        /// <response code="400">Se algum dos campos do "body" da requisicao possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="409">Se ja houver uma relacao com ID fornecido</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(PersonComputer), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<ActionResult<PersonComputer>> PostPersonComputer(PersonComputer personComputer)
        {
            _context.PeopleComputers.Add(personComputer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IDNotAvailable(personComputer.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPersonComputer", new { id = personComputer.ID }, personComputer);
        }

        /// <summary>
        /// Exclui uma relacao existente
        /// </summary>
        /// <param name="ID"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Delete api/ComputersPeople/abcd-12345
        /// </remarks>
        /// <returns>Retorna os detalhes da relacao excluida</returns>
        /// <response code="204">Exclui um produto existente</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="404">Se nao existe uma relacao com ID informado</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeletePersonComputer(string ID)
        {
            var personComputer = await _context.PeopleComputers.FindAsync(ID);
            if (personComputer == null)
            {
                return NotFound();
            }

            _context.PeopleComputers.Remove(personComputer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [NonAction]
        public bool IDNotAvailable(string ID)
        {
            return _context.PeopleComputers.Any(e => e.ID == ID);
        }

        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                if (encodedData.Length > 20)
                    encodedData = encodedData.Substring(0, 20);
                else
                    encodedData = EncodePasswordToBase64(encodedData);

                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in base64Encode ${ex.Message}");
            }
        }
    }
}
