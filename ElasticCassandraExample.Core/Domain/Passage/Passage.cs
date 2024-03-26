using ElasticCassandraExample.Core.Domain.Base;

namespace ElasticCassandraExample.Core.Domain.Passage
{
    public class Passage
    {
        public int IdMonth { get; set; }
        public DateTimeOffset DateTimePassage { get; set; }
        public string CodeLane { get; set; }
        public sbyte Axles { get; set; }
        public string CodeEquipment { get; set; }
        // public SByte Lane { get; set; }
        public string LaneDirection { get; set; }
        public string LicensePlate { get; set; }
        public int SizeVehicle { get; set; }
        public int SpeedMeasured { get; set; }
        public string UrlImage { get; set; }
        public string VehicleClassification { get; set; }
        public string BrandVehicle { get; set; }
        public string ColorVehicle { get; set; }
        public string ModelVehicle { get; set; }
    }
}
