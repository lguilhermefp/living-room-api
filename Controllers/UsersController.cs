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
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        public UsersController (AppDbContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this._context = context;
        }

        /// <summary>
        /// Retorna um token de acesso a api para usuarios autorizados
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Um token de acesso a api para usuarios autorizados</returns>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Post api/Users/authenticate
        ///         
        ///     No "body", insira os campos ID, Name, Email e Password
        /// 
        ///     Usuario generico (nao pode ser apagado):
        ///         { "ID": "admin-1234", "Name": "admin", "Email": "admin@example.com", "Password": "admin123" }
        ///     
        /// </remarks>
        /// <response code="200">Retorna um token de acesso a api</response>
        /// <response code="400">Se algum campo inserido possui valor invalido</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] User user)
        {
            try {
                var resultToken = await jwtAuthenticationManager.AuthenticateAsync(_context, user.ID, user.Password);
                if (resultToken == null)
                    return Unauthorized();

                return Ok(resultToken);
            }
            catch
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Retorna uma lista com detalhes de todos os usuarios
        /// </summary>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Get api/Users/
        /// </remarks>
        /// <returns>Lista com detalhes de todos os usuarios</returns>
        /// <response code="200">Retorna a lista requisitada</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se houve falha na conexao com o banco de dados</response>
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Retorna os detalhes do usuario requisitado
        /// </summary>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         GET api/Users/abcd-12345
        /// </remarks>
        /// <param name="ID"></param>
        /// <returns>Os detalhes do usuario requisitado</returns>
        /// <response code="200">Retorna o usuario requisitado</response>
        /// <response code="400">Se não houver usuario com essa identificacao</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("{ID}")]
        public async Task<ActionResult<User>> GetUser(string ID)
        {
            var user = await _context.Users.FindAsync(ID);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Cria um novo usuario
        /// </summary>
        /// <param name="user"></param>
        /// <remarks>
        ///     Requisicao padrao:
        ///         Post api/Users/
        ///         
        ///         No "body", insira os campos ID, Name, Email e Password
        ///         
        ///         Campo "ID" precisa ter 10 caracteres
        /// </remarks>
        /// <returns>Os detalhes do usuario criado</returns>
        /// <response code="201">Retorna os detalhes do usuario criado</response>
        /// <response code="400">Se algum dos campos do "body" da requisicao possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="409">Se ja houver um usuario com ID ou Email fornecidos</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.Password = EncodePasswordToBase64(user.Password);
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IDOrEmailNotAvailable(user.ID, user.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }

        /// <summary>
        /// Altera informacoes de um usuario existente
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="user"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Put api/User/abcd-12345
        ///         
        ///         No "body", insira seu ID e a atualizacao dos campos Name, Email e Password
        /// </remarks>
        /// <response code="204">Atualiza o usuario requisitado</response>
        /// <response code="400">Se nao existe usuario com ID informado ou "body" possui valor invalido ou o formato e ilegivel</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpPut("{ID}")]
        public async Task<IActionResult> PutUser(string ID, User user)
        {
            user.Password = EncodePasswordToBase64(user.Password);
            if (ID != user.ID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

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
        /// Exclui um usuario existente
        /// </summary>
        /// <param name="ID"></param>
        /// <remarks>
        ///     Exemplo de requisicao:
        ///         Delete api/Users/abcd-12345
        /// </remarks>
        /// <returns>Retorna os detalhes do usuario excluido</returns>
        /// <response code="204">Exclui um usuario existente</response>
        /// <response code="401">Se o autor da requisicao nao possui autorizacao</response>
        /// <response code="404">Se nao existe usuario com ID informado</response>
        /// <response code="500">Se o banco de dados retornou erro</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteUser(string ID)
        {
			if(ID == "admin-123")
				return BadRequest("O usuario generico nao pode ser apagado");

            var user = await _context.Users.FindAsync(ID);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [NonAction]
        public bool IDOrEmailNotAvailable(string ID, string email)
        {
			return IDNotAvailable(ID) || EmailNotAvailable(email);
        }

        [NonAction]
        public bool IDNotAvailable(string ID)
        {
            return _context.Users.Any(e => e.ID == ID);
        }

		[NonAction]
        public bool EmailNotAvailable(string email)
        {
            return _context.Users.Any(e => e.Email == email);
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
