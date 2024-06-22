namespace MutualWeb.Shared.DTOs

{
    public class PaginationDTO
    {
        public int Id { get; set; }

        public int Page { get; set; } = 1;

        public int RecordsNumber { get; set; } = 10;

        public string? Filter { get; set; }

        public int? TipoClienteFilter { get; set; }

        public bool? SocioFilter { get; set; }

        public bool? BajaFilter { get; set; }
    }
}
