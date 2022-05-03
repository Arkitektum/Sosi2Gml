using Microsoft.AspNetCore.Mvc;
using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Constants;
using Sosi2Gml.Reguleringsplanforslag.Mappers;
using Sosi2Gml.Reguleringsplanforslag.Mappers.Interfaces;
using Sosi2Gml.Reguleringsplanforslag.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace Sosi2Gml.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static Regex _sosiObjectRegex = new(@"^\.[A-ZÆØÅ]+", RegexOptions.Compiled);

        private readonly IGmlFeatureMapper<RpGrense> _rpGrenseMapper;
        private readonly IGmlFeatureMapper<RpFormålGrense> _rpFormålGrenseMapper;
        private readonly IGmlFeatureMapper<RpSikringGrense> _rpSikringGrenseMapper;
        private readonly IGmlSurfaceFeatureMapper<RpOmråde, RpGrense> _rpOmrådeMapper;
        private readonly IGmlSurfaceFeatureMapper<RpArealformålOmråde, RpFormålGrense> _rpArealformålOmrådeMapper;
        private readonly IGmlSurfaceFeatureMapper<RpSikringSone, RpSikringGrense> _rpSikringSoneMapper;
        private readonly IGmlFeatureMapper<RpJuridiskPunkt> _rpJuridiskPunktMapper;
        private readonly IRpHensynGrenseMapper _rpHensynGrenseMapper;

        public TestController(
             IGmlFeatureMapper<RpGrense> rpGrenseMapper,
             IGmlFeatureMapper<RpFormålGrense> rpFormålGrenseMapper,
             //IGmlFeatureMapper<RpSikringGrense> rpSikringGrenseMapper,
             IGmlSurfaceFeatureMapper<RpOmråde, RpGrense> rpOmrådeMapper,
             IGmlSurfaceFeatureMapper<RpArealformålOmråde, RpFormålGrense> rpArealformålOmrådeMapper,
             IGmlSurfaceFeatureMapper<RpSikringSone, RpSikringGrense> rpSikringSoneMapper,
             IGmlFeatureMapper<RpJuridiskPunkt> rpJuridiskPunktMapper,
             IRpHensynGrenseMapper rpHensynGrenseMapper)
        {
            _rpGrenseMapper = rpGrenseMapper;
            _rpFormålGrenseMapper = rpFormålGrenseMapper;
           // _rpSikringGrenseMapper = rpSikringGrenseMapper;
            _rpOmrådeMapper = rpOmrådeMapper;
            _rpArealformålOmrådeMapper = rpArealformålOmrådeMapper;
            _rpJuridiskPunktMapper = rpJuridiskPunktMapper;
            _rpSikringSoneMapper = rpSikringSoneMapper;
            _rpHensynGrenseMapper = rpHensynGrenseMapper;
        }

        [HttpPost]
        public async Task<IActionResult> Sosi2Gml(IFormFile sosiFile)
        {
            const string SrsName = "http://www.opengis.net/def/crs/EPSG/0/25832";
            const int DecimalPlaces = 2;

            var sosiObjects = await ReadSosiFileAsync(sosiFile);
            var hode = sosiObjects.First();

            var rpSikringGrenseObjects = sosiObjects["RpSikringGrense"];
            var rpSikringGrenser = rpSikringGrenseObjects.ConvertAll(sosiObject => _rpHensynGrenseMapper.Map<RpSikringGrense>(sosiObject, SrsName, DecimalPlaces));

            var rpSikringSoneObjects = sosiObjects["RpSikringSone"];
            var rpSikringSoner = rpSikringSoneObjects.ConvertAll(sosiObject => _rpSikringSoneMapper.Map(sosiObject, SrsName, DecimalPlaces, rpSikringGrenser));

            var b = rpSikringSoner;

            /*var rpJuridiskPunktObjects = sosiObjects[FeatureMemberName.RpJuridiskPunkt];
            var rpJuridiskePunkt = rpJuridiskPunktObjects.ConvertAll(sosiObject => _rpJuridiskPunktMapper.Map(sosiObject, SrsName, DecimalPlaces));

            var rpGrenseSosiObjects = sosiObjects[FeatureMemberName.RpGrense];
            var rpGrenser = rpGrenseSosiObjects.ConvertAll(rpGrense => _rpGrenseMapper.Map(rpGrense, SrsName, DecimalPlaces));

            var rpOmrådeSosiObjects = sosiObjects[FeatureMemberName.RpOmråde];
            var rpOmråder = rpOmrådeSosiObjects.ConvertAll(rpOmråde => _rpOmrådeMapper.Map(rpOmråde, SrsName, DecimalPlaces, rpGrenser));

            var rpFormålGrenseSosiObjects = sosiObjects[FeatureMemberName.RpFormålGrense];
            var rpFormålGrenser = rpFormålGrenseSosiObjects.ConvertAll(rpFormålGrense => _rpFormålGrenseMapper.Map(rpFormålGrense, SrsName, DecimalPlaces));

            var rpArealformålOmrådeSosiObjects = sosiObjects[FeatureMemberName.RpArealformålOmråde];
            var rpArealformålOmråder = rpArealformålOmrådeSosiObjects.ConvertAll(rpArealformålOmråde => _rpArealformålOmrådeMapper.Map(rpArealformålOmråde, SrsName, DecimalPlaces, rpFormålGrenser));

            var rpSikringGrenseObjects = sosiObjects["RpSikringGrense"];
            var rpSikringGrenser = rpSikringGrenseObjects.ConvertAll(sosiObject => _rpSikringGrenseMapper.Map(sosiObject, SrsName, DecimalPlaces));

            var rpSikringSoneObjects = sosiObjects["RpSikringSone"];
            var rpSikringSoner = rpSikringGrenseObjects.ConvertAll(sosiObject => _rpSikringSoneMapper.Map(sosiObject, SrsName, DecimalPlaces, rpSikringGrenser));

            var b = rpArealformålOmråder.First();*/

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