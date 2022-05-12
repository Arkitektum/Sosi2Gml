using Microsoft.AspNetCore.Mvc;
using Sosi2Gml.Application.Services.MultipartRequest;
using Sosi2Gml.Reguleringsplanforslag.Services;

namespace Sosi2Gml.Web.Controllers
{
    [ApiController]
    [Route("/")]
    public class Sosi2GmlController : BaseController
    {
        private readonly IMultipartRequestService _multipartRequestService;
        private readonly IRpfSosi2GmlService _rpfSosi2GmlService;

        public Sosi2GmlController(
            IMultipartRequestService multipartRequestService,
            IRpfSosi2GmlService rpfSosi2GmlService,
            ILogger<Sosi2GmlController> logger) : base(logger)
        {
            _multipartRequestService = multipartRequestService;
            _rpfSosi2GmlService = rpfSosi2GmlService;
        }

        [HttpPost("reguleringsplanforslag")]
        public async Task<IActionResult> ConvertReguleringsplanforslag()
        {
            try
            {
                var sosiFile = await _multipartRequestService.GetFileFromMultipartAsync();

                if (sosiFile == null)
                    return BadRequest();

                var document = await _rpfSosi2GmlService.Sosi2GmlAsync(sosiFile);

                return File(document, "application/xml+gml", $"{Path.ChangeExtension(sosiFile.FileName, "gml")}");
            }
            catch (Exception exception)
            {
                var result = HandleException(exception);

                if (result != null)
                    return result;

                throw;
            }
        }
    }
}