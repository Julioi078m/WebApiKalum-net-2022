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




        [HttpPost]
        public async Task<ActionResult<Cargo>> Post([FromBody] Cargo value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar nuevo Cargo");
            value.CargoId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Cargo.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando proceso de crear nueva Cargo");
            return new CreatedAtRouteResult("GetCargo",new {id = value.CargoId}, value);
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<Cargo>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion del Cargo");
                var cargo = await DbContext.Cargo.FirstOrDefaultAsync(cc => cc.CargoId == id);
                 if(cargo== null)
                { 
                    Logger.LogWarning($"No se encontro ninguna cargo con el id {id}");
                    return NotFound();
                }
                else
                {
                    DbContext.Cargo.Remove(cargo);
                    await DbContext.SaveChangesAsync();
                    Logger.LogInformation($"Se ha eliminado correctamente el cargo con el id {id}");
                    return cargo;

                }
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Cargo value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion del cargo con el id {id}");
            Cargo Cargo = await DbContext.Cargo.FirstOrDefaultAsync(cc => cc.CargoId == id);
            if(Cargo == null)
            {
                Logger.LogWarning($"No existe el cargo con el id {id}");
                return BadRequest();
            }
            Cargo.Descripcion = value.Descripcion;
            Cargo.Prefijo = value.Prefijo;
            Cargo.Monto = value.Monto;
            Cargo.GeneraMora = value.GeneraMora;
            Cargo.PorcentajeMora = value.PorcentajeMora;



            DbContext.Entry(Cargo).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }

    }

}