using Microsoft.AspNetCore.Http;

namespace Sosi2Gml.Application.Services.MultipartRequest
{
    public interface IMultipartRequestService
    {
        Task<IFormFile> GetFileFromMultipartAsync();
    }
}
