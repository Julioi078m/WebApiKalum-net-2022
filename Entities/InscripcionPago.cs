namespace WebApiKalum.Entities
{
    public class InscripcionPago
    {
        public string NoExpediente { get; set;}
        public string BoletaPago { get; set;}
        public string Anio { get; set; }
        public DateTime FechaPago { get; set; }
        public double Monto { get; set; }

        public virtual Aspirante Aspirante { get; set; }


    }
}