using System.Collections.Generic;

namespace CarDealer.DTO
{
    public class ImportCarsInputModel
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public long TravelledDistance { get; set; }
        public IEnumerable<int> PartsId { get; set; }
    }
}