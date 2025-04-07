using ApiViajes.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ViajesMvcNetCore.Models;

namespace ApiViajes.Repositories
{
    public class RepositoryLugar
    {
        private readonly ViajesContext _context;

        public RepositoryLugar(ViajesContext context)
        {
            _context = context;
        }

        // Obtener todos los lugares
        public async Task<List<Lugar>> GetLugaresAsync()
        {
            return await _context.Lugares.ToListAsync();
        }

        // Buscar un lugar por ID
        public async Task<Lugar> FindLugarAsync(int idLugar)
        {
            return await _context.Lugares.FirstOrDefaultAsync(l => l.IdLugar == idLugar);
        }

        // Buscar lugares por nombre
        public async Task<List<Lugar>> FindLugarByNameAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<Lugar>();
            }

            string lowerSearchTerm = searchTerm.ToLower();

            return await _context.Lugares
                .Where(l => l.Nombre.ToLower().Contains(lowerSearchTerm))
                .ToListAsync();
        }

        // Obtener lugares por tipo
        public async Task<List<Lugar>> GetLugaresPorTipoAsync(string tipo)
        {
            return await _context.Lugares
                .Where(l => l.Tipo.ToLower() == tipo.ToLower())
                .ToListAsync();
        }

        // Insertar un nuevo lugar
        public async Task InsertLugarAsync(string nombre, string descripcion, string ubicacion, string categoria, DateTime horario, string imagen, string tipo)
        {
            Lugar lugar = new Lugar
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Ubicacion = ubicacion,
                Categoria = categoria,
                Horario = horario,
                Imagen = imagen,
                Tipo = tipo
            };

            await _context.Lugares.AddAsync(lugar);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLugarAsync(int idLugar, string nombre, string descripcion, string ubicacion, string categoria, DateTime horario, string imagen, string tipo)
        {
            Lugar lugar = await _context.Lugares.FirstOrDefaultAsync(l => l.IdLugar == idLugar);

            if (lugar != null)
            {
                lugar.Nombre = nombre;
                lugar.Descripcion = descripcion;
                lugar.Ubicacion = ubicacion;
                lugar.Categoria = categoria;
                lugar.Horario = horario;
                lugar.Imagen = imagen;
                lugar.Tipo = tipo;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteLugarAsync(int idLugar)
        {
            Lugar lugar = await _context.Lugares.FirstOrDefaultAsync(l => l.IdLugar == idLugar);

            if (lugar != null)
            {
                _context.Lugares.Remove(lugar);
                await _context.SaveChangesAsync();
            }
        }


        // Obtener lugares favoritos de un usuario
        public async Task<List<LugarFavorito>> GetFavoritosLugarAsync(int idUsuario)
        {
            string sql = "EXEC SP_FAVORITOS_BY_USUARIO @idusuario";
            var favoritos = await _context.LugaresFavoritos
                .FromSqlRaw(sql, new SqlParameter("@idusuario", idUsuario))
                .ToListAsync();

            return favoritos;
        }

        // Verificar si un lugar está en los favoritos de un usuario
        public async Task<bool> ExisteFavoritoAsync(int idUsuario, int idLugar)
        {
            var favorito = await _context.LugaresFavoritos
                .FirstOrDefaultAsync(f => f.IdUsuario == idUsuario && f.IdLugar == idLugar);

            return favorito != null;
        }

        // Agregar un lugar a los favoritos
        public async Task AddFavoritoAsync(int idUsuario, int idLugar, DateTime fecha, string imagen, string nombre, string descripcion, string ubicacion, string tipo)
        {
            LugarFavorito favorito = new LugarFavorito
            {
                IdUsuario = idUsuario,
                IdLugar = idLugar,
                FechaDeVisitaLugar = fecha,
                ImagenLugar = imagen,
                NombreLugar = nombre,
                DescripcionLugar = descripcion,
                UbicacionLugar = ubicacion,
                TipoLugar = tipo
            };

            await _context.LugaresFavoritos.AddAsync(favorito);
            await _context.SaveChangesAsync();
        }


        // Eliminar un lugar de los favoritos
        public async Task DeleteFavoritoAsync(int idUsuario, int idLugar)
        {
            LugarFavorito favorito = await _context.LugaresFavoritos
                .FirstOrDefaultAsync(f => f.IdUsuario == idUsuario && f.IdLugar == idLugar);

            if (favorito != null)
            {
                _context.LugaresFavoritos.Remove(favorito);
                await _context.SaveChangesAsync();
            }
        }


        // Obtener los comentarios de un lugar
        public async Task<List<Comentario>> GetComentariosLugarAsync(int idLugar)
        {
            string sql = "EXEC SP_GET_COMENTARIOS_LUGAR @idLugar";

            var consulta = _context.Comentarios
                .FromSqlRaw(sql, new SqlParameter("@idLugar", idLugar));

            return await consulta.ToListAsync();
        }

        // Agregar un comentario a un lugar
        public async Task AddComentarioAsync(int idLugar, int idUsuario, string comentario)
        {
            Comentario nuevoComentario = new Comentario
            {
                IdLugar = idLugar,
                IdUsuario = idUsuario,
                Comentarios = comentario,
                FechaComentario = DateTime.Now // Suponiendo que la fecha es la actual
            };

            await _context.Comentarios.AddAsync(nuevoComentario);
            await _context.SaveChangesAsync();
        }


        // Eliminar un comentario
        public async Task DeleteComentarioAsync(int idComentario)
        {
            Comentario comentario = await _context.Comentarios.FirstOrDefaultAsync(c => c.IdComentario == idComentario);

            if (comentario != null)
            {
                _context.Comentarios.Remove(comentario);
                await _context.SaveChangesAsync();
            }
        }

    }
}
