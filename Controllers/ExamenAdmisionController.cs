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

    }

}