using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumno")]

    public class AlumnoController : ControllerBase

    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;

        public AlumnoController(KalumDbContext _DbContext,ILogger<AlumnoController> _Logger )
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoController>>> Get()
        {
            List<Alumno> alumno = null;

            Logger.LogDebug("Iniciando proceso de consulta de carreras tecnicas en la base de datos");
            alumno = await DbContext.Alumno.Include(c => c.Inscripciones).Include(c => c.CuentaXCobrar).ToListAsync();
            if (alumno == null || alumno.Count == 0)
            {
                Logger.LogWarning("No existe carrera tecnica");
                return new NoContentResult();
            }

            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(alumno);

        }



        [HttpGet("{id}", Name = "GetAlumno")]
        public async Task<ActionResult<Alumno>> GetJornada  (string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id" + id);
            var alumno = await DbContext.Alumno.Include(c => c.Inscripciones).Include(c => c.CuentaXCobrar).FirstOrDefaultAsync(ct => ct.Carne == id);
            if (alumno == null)
            {
                Logger.LogWarning("No existe la carrera tecnica con el id" + id);
                return new NoContentResult();

            }
            Logger.LogInformation("Finalizando proceso de busqueda de forma exitosa");
            return Ok(alumno);
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