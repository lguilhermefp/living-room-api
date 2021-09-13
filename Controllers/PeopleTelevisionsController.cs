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
        /// Retorna uma lista com detalhes de todos as pessoas relacoes entre pessoas e computadores
        /// </summary>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Get api/PeopleTelevision/
        /// </remarks>
        /// <returns>Lista com detalhes de todos as pessoas relacoes entre pessoas e computadores</returns>
        /// <response code="200">Retorna a lista requisitada</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se houve falha na conexao com o banco de dados</response>
        [ProducesResponseType(typeof(IEnumerable<PersonTelevision>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonTelevision>>> GetPersonTelevision()
        {
            return await _context.PeopleTelevisions.ToListAsync();
        }

        /// <summary>
        /// Retorna os detalhes da pessoa requisitada
        /// </summary>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         GET api/People/abcd-12345
        /// </remarks>
        /// <param name="personId"></param>
        /// <returns>Os detalhes da pessoa requisitada</returns>
        /// <response code="200">Retorna a pessoa requisitada</response>
        /// <response code="400">Se não houver pessoa com esse ID</response>
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
        /// Retorna os detalhes da pessoa requisitada
        /// </summary>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         GET api/People/abcd-12345
        /// </remarks>
        /// <param name="televisionId"></param>
        /// <returns>Os detalhes da pessoa requisitada</returns>
        /// <response code="200">Retorna a pessoa requisitada</response>
        /// <response code="400">Se não houver pessoa com esse ID</response>
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
        /// Altera informacoes de uma pessoa existente
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="personTelevision"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Put api/People/abcd-12345
        ///         
        ///         No "body", insira sua ID e a atualizacao dos campos Nome, Email e Senha
        /// </remarks>
        /// <response code="204">Atualiza a pessoa requisitado</response>
        /// <response code="400">Se nao existe pessoa com ID informada ou "body" possui valor invalido ou o formato e ilegivel</response>
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
        /// Cria uma nova pessoa
        /// </summary>
        /// <param name="personTelevision"></param>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Post api/People/
        ///         
        ///         No "body", insira os campos ID, Name, Email e Password
        ///         
        ///         Campo "ID" precisa ter 10 caracteres
        /// </remarks>
        /// <returns>Os detalhes da pessoa criada</returns>
        /// <response code="201">Retorna os detalhes da pessoa criada</response>
        /// <response code="400">Se algum dos campos do "body" da requisicao possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="409">Se ja houver uma pessoa com ID ou Email fornecidos</response>
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
        /// Exclui uma pessoa existente
        /// </summary>
        /// <param name="ID"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Delete api/TelevisionsPeople/abcd-12345
        /// </remarks>
        /// <returns>Retorna os detalhes da pessoa excluido</returns>
        /// <response code="204">Exclui um produto existente</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="404">Se nao existe uma pessoa com ID informado</response>
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
