namespace librarwebapp.Models.ExternalServices.Suscriptions
{
    public class SuscripcionDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int PeriodoMeses { get; set; }
        public decimal PrecioPorMes { get; set; }
        public decimal Total { get; set; }
        public EstadoSuscripcion Estado { get; set; }
    }
}
