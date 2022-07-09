using System.ComponentModel.DataAnnotations;


namespace WebApiKalum.Entities
{
    public class ExamenAdmision
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string ExamenId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(128,MinimumLength = 5, ErrorMessage = "La cantidad minima de caracteres es {2} y maxima es {1} para el campo {0}")]
        public DateTime FechaExamen { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
        
    }
}