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
        private static Regex _sosiObjectRegex = new(@"^\.[A-Z���]+", RegexOptions.Compiled);

        private readonly IGmlFeatureMapper<RpGrense> _rpGrenseMapper;
        private readonly IGmlFeatureMapper<RpForm�lGrense> _rpForm�lGrenseMapper;
        private readonly IGmlFeatureMapper<RpSikringGrense> _rpSikringGrenseMapper;
        private readonly IGmlSurfaceFeatureMapper<RpOmr�de, RpGrense> _rpOmr�deMapper;
        private readonly IGmlSurfaceFeatureMapper<RpArealform�lOmr�de, RpForm�lGrense> _rpArealform�lOmr�deMapper;
        private readonly IGmlSurfaceFeatureMapper<RpSikringSone, RpSikringGrense> _rpSikringSoneMapper;
        private readonly IGmlFeatureMapper<RpJuridiskPunkt> _rpJuridiskPunktMapper;
        private readonly IRpHensynGrenseMapper _rpHensynGrenseMapper;

        public TestController(
             IGmlFeatureMapper<RpGrense> rpGrenseMapper,
             IGmlFeatureMapper<RpForm�lGrense> rpForm�lGrenseMapper,
             //IGmlFeatureMapper<RpSikringGrense> rpSikringGrenseMapper,
             IGmlSurfaceFeatureMapper<RpOmr�de, RpGrense> rpOmr�deMapper,
             IGmlSurfaceFeatureMapper<RpArealform�lOmr�de, RpForm�lGrense> rpArealform�lOmr�deMapper,
             IGmlSurfaceFeatureMapper<RpSikringSone, RpSikringGrense> rpSikringSoneMapper,
             IGmlFeatureMapper<RpJuridiskPunkt> rpJuridiskPunktMapper,
             IRpHensynGrenseMapper rpHensynGrenseMapper)
        {
            _rpGrenseMapper = rpGrenseMapper;
            _rpForm�lGrenseMapper = rpForm�lGrenseMapper;
           // _rpSikringGrenseMapper = rpSikringGrenseMapper;
            _rpOmr�deMapper = rpOmr�deMapper;
            _rpArealform�lOmr�deMapper = rpArealform�lOmr�deMapper;
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

            var rpOmr�deSosiObjects = sosiObjects[FeatureMemberName.RpOmr�de];
            var rpOmr�der = rpOmr�deSosiObjects.ConvertAll(rpOmr�de => _rpOmr�deMapper.Map(rpOmr�de, SrsName, DecimalPlaces, rpGrenser));

            var rpForm�lGrenseSosiObjects = sosiObjects[FeatureMemberName.RpForm�lGrense];
            var rpForm�lGrenser = rpForm�lGrenseSosiObjects.ConvertAll(rpForm�lGrense => _rpForm�lGrenseMapper.Map(rpForm�lGrense, SrsName, DecimalPlaces));

            var rpArealform�lOmr�deSosiObjects = sosiObjects[FeatureMemberName.RpArealform�lOmr�de];
            var rpArealform�lOmr�der = rpArealform�lOmr�deSosiObjects.ConvertAll(rpArealform�lOmr�de => _rpArealform�lOmr�deMapper.Map(rpArealform�lOmr�de, SrsName, DecimalPlaces, rpForm�lGrenser));

            var rpSikringGrenseObjects = sosiObjects["RpSikringGrense"];
            var rpSikringGrenser = rpSikringGrenseObjects.ConvertAll(sosiObject => _rpSikringGrenseMapper.Map(sosiObject, SrsName, DecimalPlaces));

            var rpSikringSoneObjects = sosiObjects["RpSikringSone"];
            var rpSikringSoner = rpSikringGrenseObjects.ConvertAll(sosiObject => _rpSikringSoneMapper.Map(sosiObject, SrsName, DecimalPlaces, rpSikringGrenser));

            var b = rpArealform�lOmr�der.First();*/

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