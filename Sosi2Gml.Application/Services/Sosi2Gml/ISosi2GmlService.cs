using Microsoft.AspNetCore.Http;

namespace Sosi2Gml.Application.Services.Sosi2Gml
{
    public interface ISosi2GmlService
    {
        Task<MemoryStream> Sosi2GmlAsync(IFormFile sosiFile);
    }
}
