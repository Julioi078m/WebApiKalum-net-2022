using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Cargo")]

    public class CargoController : ControllerBase

    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CargoController> Logger;

        public CargoController(KalumDbContext _DbContext,ILogger<CargoController> _Logger )
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargoController>>> Get()
        {
            List<Cargo> cargo = null;

            Logger.LogDebug("Iniciando proceso de consulta de carreras tecnicas en la base de datos");
            cargo = await DbContext.Cargo.Include(c => c.CuentaXCobrar).ToListAsync();
            if (cargo == null || cargo.Count == 0)
            {
                Logger.LogWarning("No existe carrera tecnica");
                return new NoContentResult();
            }

            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(cargo);

        }



        [HttpGet("{id}", Name = "GetCargo")]
        public async Task<ActionResult<Cargo>> GetCargo  (string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id" + id);
            var cargo = await DbContext.Cargo.Include(c => c.CuentaXCobrar).FirstOrDefaultAsync(ct => ct.CargoId == id);
            if (cargo == null)
            {
                Logger.LogWarning("No existe la carrera tecnica con el id" + id);
                return new NoContentResult();

            }
            Logger.LogInformation("Finalizando proceso de busqueda de forma exitosa");
            return Ok(cargo);
        }

    }

}