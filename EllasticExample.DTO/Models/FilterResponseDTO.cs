namespace EllasticExample.DTO.Models
{
    public class FilterResponseDTO<T>
    {
        public string PaginationScroolId { get; set; }
        public List<T> Items { get; set; }
    }
}
