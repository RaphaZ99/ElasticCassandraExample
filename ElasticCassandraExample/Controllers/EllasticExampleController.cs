using ElasticCassandraExample.Application.Interfaces;
using ElasticCassandraExample.Core.Domain.Passage;
using EllasticExample.DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElasticCassandraExample.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class EllasticExampleController : ControllerBase
    {

        private readonly IPassageService _passageService;

        public EllasticExampleController(IPassageService passageService)
        {
            _passageService = passageService;
        }

        [HttpPost]
        public async Task<IActionResult> PostPassage()
        {
            var listPassage = new List<Passage>();
            using (var reader = new StreamReader("C:\\Users\\RaphaelMotaSantana\\Downloads\\axcross_passage._3Months.csv"))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var fields = line.Split(',');

                    listPassage.Add(new Passage
                    {
                        IdMonth = Convert.ToInt32(fields[0]),
                        DateTimePassage = DateTimeOffset.Parse(fields[1]),
                        CodeLane = fields[2],
                        Axles = Convert.ToSByte(fields[3]),
                        CodeEquipment = fields[4],
                        //Lane = Convert.ToSByte(fields[5]),
                        LaneDirection = fields[6],
                        LicensePlate = fields[7],
                        SizeVehicle = int.Parse(fields[8]),
                        SpeedMeasured = int.Parse(fields[9]),
                        UrlImage = fields[10],
                        VehicleClassification = fields[11],
                        BrandVehicle = fields[12],
                        ColorVehicle = fields[13],
                        ModelVehicle = fields[14]
                    });

                }

                await _passageService.InsertDataAsync(listPassage);

            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPassages()
        {

            var passages = await _passageService.GetAllAsync();

            return Ok(passages);
        }

        [HttpGet("{licensePlate}")]
        public async Task<ActionResult> GetPassagesByLicensePlateAsync(string licensePlate)
        {
            return Ok(_passageService.GetPassagesByLicensePlateAsync(licensePlate));
        }

        [HttpDelete("{indexName}")]
        public async Task<IActionResult> DeleteAllDocumentsByIndexNameAsync(string indexName)
        {
            return Ok(await _passageService.DeleteAllDocumentsByIndexNameAsync(indexName));
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetByDateMonthEquipmentCode([FromBody] FilterDTO filterDto)
        {
            return Ok(await _passageService.GetByDateMonthEquipmentCode(filterDto.StartDate, filterDto.EndDate, filterDto.IdMonth, filterDto.CodeEquipment));
        }

        [HttpPost("filter/{paginationScrollId}")]
        public async Task<IActionResult> GetBySearchByPaginationScroll(string paginationScrollId)
        {
            return Ok(await _passageService.GetSearchByScrollId("1h", paginationScrollId));
        }
    }
}
