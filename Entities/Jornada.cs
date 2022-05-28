namespace WebApiKalum.Entities
{
    public class Jornada
    {
        public string JornadaId { get; set; }
        public string Jorny { get; set; }
        public string Descripcion { get; set; }

        public virtual List<Aspirante> Aspirante { get; set;}

    }
}