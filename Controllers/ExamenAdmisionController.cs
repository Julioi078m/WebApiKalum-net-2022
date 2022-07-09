using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/ExamenAdmision")]

    public class ExamenAdmisionController : ControllerBase

    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;

        public ExamenAdmisionController(KalumDbContext _DbContext,ILogger<ExamenAdmisionController> _Logger )
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionController>>> Get()
        {
            List<ExamenAdmision> examenAdmision = null;

            Logger.LogDebug("Iniciando proceso de consulta de carreras tecnicas en la base de datos");
            examenAdmision = await DbContext.ExamenAdmision.Include(c => c.Aspirantes).ToListAsync();
            if (examenAdmision == null || examenAdmision.Count == 0)
            {
                Logger.LogWarning("No existe carrera tecnica");
                return new NoContentResult();
            }

            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(examenAdmision);

        }



        [HttpGet("{id}", Name = "GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmision>> GetJornada  (string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id" + id);
            var examenAdmision = await DbContext.ExamenAdmision.Include(c => c.Aspirantes).FirstOrDefaultAsync(ct => ct.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning("No existe la carrera tecnica con el id" + id);
                return new NoContentResult();

            }
            Logger.LogInformation("Finalizando proceso de busqueda de forma exitosa");
            return Ok(examenAdmision);
        }

       [HttpPost]
        public async Task<ActionResult<ExamenAdmision>> Post([FromBody] ExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar nueva fecha de examen de ExamenAdmision");
            value.ExamenId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.ExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando proceso de crear nueva fecha de examen de admision");
            return new CreatedAtRouteResult("GetExamenAdmision",new {id = value.ExamenId}, value);
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<ExamenAdmision>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion de la fecha de examen de admision");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if(examenAdmision == null)
            { 
                Logger.LogWarning($"No se encontro ninguna fecha con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.ExamenAdmision.Remove(examenAdmision);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la fecha con el id {id}");
                return examenAdmision;

            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de la fecha con el id {id}");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if(examenAdmision == null)
            {
                Logger.LogWarning($"No existe la carrera tecnica con el id {id}");
                return BadRequest();
            }
            examenAdmision.FechaExamen = value.FechaExamen;
            DbContext.Entry(examenAdmision).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }

    }

}