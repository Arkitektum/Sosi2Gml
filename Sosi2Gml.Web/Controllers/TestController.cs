using Microsoft.AspNetCore.Mvc;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Constants;
using Sosi2Gml.Reguleringsplanforslag.Mappers.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace Sosi2Gml.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static Regex _sosiObjectRegex = new(@"^\.[A-Z∆ÿ≈]+", RegexOptions.Compiled);

        private readonly IRpGrenseMapper _rpGrenseMapper;

        public TestController(
             IRpGrenseMapper rpGrenseMapper)
        {
            _rpGrenseMapper = rpGrenseMapper;
        }

        [HttpPost]
        public async Task<IActionResult> Sosi2Gml(IFormFile sosiFile)
        {
            var sosiObjects = await ReadSosiFileAsync(sosiFile);
            var hode = sosiObjects.First();
            var rpGrenserSosiObjects = sosiObjects[FeatureMemberName.RpGrense];
            var rpGrenser = rpGrenserSosiObjects.ConvertAll(rpGrense => _rpGrenseMapper.Map(rpGrense));

            using var rpGrense = rpGrenser.First();

            return Ok();
        }

        private static async Task<Dictionary<string, List<SosiObject>>> ReadSosiFileAsync(IFormFile sosiFile)
        {
            using var streamReader = new StreamReader(sosiFile.OpenReadStream(), Encoding.UTF8);
            var sosiLines = new Dictionary<string, List<string>>();
            string line;
            
            while ((line = await streamReader.ReadLineAsync()) != null)
            {
                if (_sosiObjectRegex.IsMatch(line) && !line.Equals(".SLUTT"))
                    sosiLines.Add(line, new List<string>());
                else
                    sosiLines.Last().Value.Add(line);
            }

            var sosiObjects = sosiLines
                .Select(kvp => SosiObject.Create(kvp.Key, kvp.Value))
                .GroupBy(sosiObject => sosiObject.GetValue("..OBJTYPE") ?? sosiObject.ElementType)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Select(sosiObject => sosiObject).ToList());

            return sosiObjects;
        }
    }
}