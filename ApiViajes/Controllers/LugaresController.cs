using ApiViajes.Models;
using ApiViajes.Repositories;
using Microsoft.AspNetCore.Mvc;
using ViajesMvcNetCore.Models;

namespace ApiViajes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LugaresController : ControllerBase
    {
        private readonly RepositoryLugar _repository;

        public LugaresController(RepositoryLugar repository)
        {
            _repository = repository;
        }

        // Obtener todos los lugares
        [HttpGet]
        public async Task<ActionResult<List<Lugar>>> GetLugares()
        {
            var lugares = await _repository.GetLugaresAsync();
            return Ok(lugares);
        }

        // Obtener un lugar específico por ID
        [HttpGet("{idLugar}")]
        public async Task<ActionResult<Lugar>> GetLugar(int idLugar)
        {
            var lugar = await _repository.FindLugarAsync(idLugar);

            if (lugar == null)
            {
                return NotFound();
            }

            return Ok(lugar);
        }

        // Buscar lugares por nombre
        [HttpGet("buscar")]
        public async Task<ActionResult<List<Lugar>>> BuscarLugares([FromQuery] string searchTerm)
        {
            var lugares = await _repository.FindLugarByNameAsync(searchTerm);
            return Ok(lugares);
        }

        // Obtener lugares por tipo
        [HttpGet("tipo/{tipo}")]
        public async Task<ActionResult<List<Lugar>>> GetLugaresPorTipo(string tipo)
        {
            var lugares = await _repository.GetLugaresPorTipoAsync(tipo);
            return Ok(lugares);
        }

        // Obtener lugares favoritos de un usuario
        [HttpGet("favoritos/{idUsuario}")]
        public async Task<ActionResult<List<LugarFavorito>>> GetFavoritos(int idUsuario)
        {
            var favoritos = await _repository.GetFavoritosLugarAsync(idUsuario);
            return Ok(favoritos);
        }

        // Obtener los comentarios de un lugar
        [HttpGet("{idLugar}/comentarios")]
        public async Task<ActionResult<List<Comentario>>> GetComentarios(int idLugar)
        {
            var comentarios = await _repository.GetComentariosLugarAsync(idLugar);
            return Ok(comentarios);
        }

        // POST - Insertar un nuevo lugar
        [HttpPost]
        public async Task<ActionResult> InsertLugar([FromBody] Lugar lugar)
        {
            if (lugar == null)
            {
                return BadRequest("El lugar no puede ser nulo.");
            }

            await _repository.InsertLugarAsync(lugar.Nombre, lugar.Descripcion, lugar.Ubicacion, lugar.Categoria, lugar.Horario, lugar.Imagen, lugar.Tipo);
            return CreatedAtAction(nameof(GetLugar), new { idLugar = lugar.IdLugar }, lugar);
        }

        // POST - Agregar un lugar a los favoritos
        [HttpPost("favoritos")]
        public async Task<ActionResult> AddFavorito([FromBody] LugarFavorito favorito)
        {
            var lugar = await _repository.FindLugarAsync(favorito.IdLugar);
            if (lugar == null)
            {
                return NotFound("Lugar no encontrado.");
            }

            var existeFavorito = await _repository.ExisteFavoritoAsync(favorito.IdUsuario, favorito.IdLugar);
            if (existeFavorito)
            {
                return Conflict("Este lugar ya está en los favoritos.");
            }

            await _repository.AddFavoritoAsync(favorito.IdUsuario, favorito.IdLugar, favorito.FechaDeVisitaLugar, lugar.Imagen, lugar.Nombre, lugar.Descripcion, lugar.Ubicacion, lugar.Tipo);
            return CreatedAtAction(nameof(GetLugar), new { idLugar = favorito.IdLugar }, favorito);
        }

        // POST - Agregar un comentario a un lugar
        [HttpPost("{idLugar}/comentarios")]
        public async Task<ActionResult> AddComentario(int idLugar, [FromBody] Comentario comentario)
        {
            if (comentario == null)
            {
                return BadRequest("Comentario no puede ser nulo.");
            }

            // Verifica si el lugar existe
            var lugar = await _repository.FindLugarAsync(idLugar);
            if (lugar == null)
            {
                return NotFound("Lugar no encontrado.");
            }

            // Asignar el ID de usuario y la fecha del comentario
            comentario.IdLugar = idLugar;
            comentario.FechaComentario = DateTime.Now;  // Suponiendo que la fecha es la actual

            // Llamamos al repositorio para agregar el comentario
            await _repository.AddComentarioAsync(idLugar, comentario.IdUsuario, comentario.Comentarios);
            return CreatedAtAction(nameof(GetComentarios), new { idLugar = idLugar }, comentario);
        }

        // PUT - Actualizar un lugar (si fuera necesario, dependiendo de los requisitos del negocio)
        // No está implementado actualmente en tu código, pero si lo necesitas, deberías agregar un PUT similar al siguiente:
        // [HttpPut("{idLugar}")]
        // public async Task<ActionResult> UpdateLugar(int idLugar, [FromBody] Lugar lugar)
        // {
        //    var existingLugar = await _repository.FindLugarAsync(idLugar);
        //    if (existingLugar == null) return NotFound();
        //    await _repository.UpdateLugarAsync(idLugar, lugar.Nombre, lugar.Descripcion, lugar.Ubicacion, lugar.Categoria, lugar.Horario, lugar.Imagen, lugar.Tipo);
        //    return NoContent();
        // }

        // DELETE - Eliminar un lugar
        [HttpDelete("{idLugar}")]
        public async Task<ActionResult> DeleteLugar(int idLugar)
        {
            var lugar = await _repository.FindLugarAsync(idLugar);
            if (lugar == null)
            {
                return NotFound("Lugar no encontrado.");
            }

            await _repository.DeleteLugarAsync(idLugar);
            return NoContent();
        }

        // DELETE - Eliminar un lugar de los favoritos
        [HttpDelete("favoritos/{idUsuario}/{idLugar}")]
        public async Task<ActionResult> DeleteFavorito(int idUsuario, int idLugar)
        {
            await _repository.DeleteFavoritoAsync(idUsuario, idLugar);
            return NoContent();
        }

        // DELETE - Eliminar un comentario
        [HttpDelete("comentarios/{idComentario}")]
        public async Task<ActionResult> DeleteComentario(int idComentario)
        {
            await _repository.DeleteComentarioAsync(idComentario);
            return NoContent();
        }
    }
}
