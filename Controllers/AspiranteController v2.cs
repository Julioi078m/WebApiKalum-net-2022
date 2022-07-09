using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using AutoMapper;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Aspirante")]

    public class AspiranteController : ControllerBase

    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AspiranteController> Logger;
        private readonly IMapper Mapper;

        public AspiranteController(KalumDbContext _DbContext,ILogger<AspiranteController> _Logger, IMapper _Mapper )
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
            
        [HttpPost]
        public async Task<ActionResult<Aspirante>> Post([FromBody] Aspirante value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar nuevo aspirante");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carreraTecnica == null)
            {
                Logger.LogInformation($"No existe la carrea tecnica con el id {value.CarreraId}");
                return BadRequest ();
            }
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync( j => j.JornadaId == value.JornadaId );
            if(jornada == null)
            {
                Logger.LogInformation($"No existe la jornada con el id {value.JornadaId}");
                return BadRequest ();
            }
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == value.ExamenId);
            if (examenAdmision == null)
            {
                Logger.LogInformation($"No existe el examen de admision con el id {value.ExamenId}");
                return BadRequest();
            }
            await DbContext.Aspirante.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se ha creado el aspirante con exito");
            return Ok(value);


        }

        [HttpGet]
        [ServiceFilter(typeof(ActionFilter))]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando el proceso de consulta de aspirantes");
            List<Aspirante> lista = await DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).Include(a => a.ExamenAdmision).ToListAsync();
            if(lista == null || lista.Count == 0 )
            {
                Logger.LogWarning("No existen registros en la base de datos");
                return new NoContentResult();

            }
            List<AspiranteListDTO> aspirantes = Mapper.Map<List<AspiranteListDTO>>(lista);
            Logger.LogInformation("La Consulta se ejecuto con exito");
            return Ok(aspirantes);

        }
    }
}
