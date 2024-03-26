namespace EllasticExample.DTO.Models
{
    public class FilterDTO
    {
        public List<int> IdMonths { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CodeEquipment { get; set; }
    }
}
