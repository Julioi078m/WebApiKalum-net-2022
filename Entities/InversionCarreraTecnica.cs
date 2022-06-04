namespace WebApiKalum.Entities
{
    public class InversionCarreraTecnica
    {
        public string InversionId { get; set; }
        public string CarreraId { get; set; }
        public double MontoInscripcion { get; set; }
        public int NumeroPagos { get; set; }
        public double MontoPago { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }

    }
}