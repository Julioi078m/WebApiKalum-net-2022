using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Jornada")]

    public class JornadaController : ControllerBase

    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;

        public JornadaController(KalumDbContext _DbContext,ILogger<JornadaController> _Logger )
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JornadaController>>> Get()
        {
            List<Jornada> jornada = null;

            Logger.LogDebug("Iniciando proceso de consulta de carreras tecnicas en la base de datos");
            jornada = await DbContext.Jornada.Include(c => c.Aspirantes).Include( c => c.Inscripciones).ToListAsync();
            if (jornada == null || jornada.Count == 0)
            {
                Logger.LogWarning("No existe carrera tecnica");
                return new NoContentResult();
            }

            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(jornada);

        }



        [HttpGet("{id}", Name = "GetJornada")]
        public async Task<ActionResult<CarreraTecnica>> GetJornada  (string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id" + id);
            var jornada = await DbContext.Jornada.Include(c => c.Aspirantes).Include( c => c.Inscripciones).FirstOrDefaultAsync(ct => ct.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning("No existe la carrera tecnica con el id" + id);
                return new NoContentResult();

            }
            Logger.LogInformation("Finalizando proceso de busqueda de forma exitosa");
            return Ok(jornada);
        }



        [HttpPost]
        public async Task<ActionResult<Jornada>> Post([FromBody] Jornada value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar nueva Jornada");
            value.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando proceso de crear nueva Jornada");
            return new CreatedAtRouteResult("GetCarreraTecnica",new {id = value.JornadaId}, value);
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<Jornada>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de la jornada");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null)
            { 
                Logger.LogWarning($"No se encontro ninguna Jornada con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.Jornada.Remove(jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la carrera tecnica con el id {id}");
                return jornada;

            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Jornada value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de la Jornada con el id {id}");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning($"No existe la Jornada con el id {id}");
                return BadRequest();
            }
            jornada.Jorna = value.Jorna;
            jornada.Descripcion = value.Descripcion;
          
            DbContext.Entry(jornada).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }
    }

}