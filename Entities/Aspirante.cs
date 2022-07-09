using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class Aspirante //: IValidatableObject
    {
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(12,MinimumLength = 12, ErrorMessage = "El campo numero de expediente de ser de 12 caracteres")]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido ")]

        public string Apellidos { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido ")]
        public string Nombres { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido ")]
        public string Direccion { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido ")]
        public string Telefono { get; set; }
        [EmailAddress(ErrorMessage ="El correo electronico no es valido ")]
        public string Email { get; set; }
         
        public string Estatus { get; set; } = "NO ASIGNADO";
        public string CarreraId { get; set; }
        public string JornadaId { get; set; }
        public string ExamenId { get; set;}
        public CarreraTecnica CarreraTecnica { get; set; }
        public Jornada Jornada { get; set; }
        public virtual ExamenAdmision ExamenAdmision { get; set; }
        public virtual List<InscripcionPago> InscripcionesPago { get; set; }
        public virtual List<ResultadoExamenAdmision> ResultadoExamenAdmision { get; set; }


        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        { 
            //bool expedienteValid = false;
            //indexof busca el dato dentro del string
            if(!string.IsNullOrEmpty(NoExpediente))
            { 
               

                if (!NoExpediente.Contains("-"))
                { 
                    yield return new ValidationResult("El numero de expediente es invalido", new string []{nameof(NoExpediente)});
                }
                else 
                {
                    int guion = NoExpediente.IndexOf("-");
                    string exp = NoExpediente.Substring(0,guion);
                    string numero = NoExpediente.Substring(guion+1,NoExpediente.Length - 4);
                    
                    if(!exp.ToUpper().Equals("EXP") || !Information.IsNumeric(numero))
                    { 
                        yield return new ValidationResult("El numero de expediente no contiene la nomenclatura adecuada", new string []{nameof(NoExpediente)});
                    }
                }
                
                
            }*/

    }

        


}
