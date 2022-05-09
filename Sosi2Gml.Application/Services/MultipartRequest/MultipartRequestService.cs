using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Sosi2Gml.Application.Services.MultipartRequest
{
    public class MultipartRequestService : IMultipartRequestService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MultipartRequestService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IFormFile> GetFileFromMultipartAsync()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var reader = new MultipartReader(request.GetMultipartBoundary(), request.Body);
            MultipartSection section;

            try
            {
                while ((section = await reader.ReadNextSectionAsync()) != null)
                {
                    if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition) &&
                        contentDisposition.IsFileDisposition() &&
                        contentDisposition.Name.Value == "sosiFile" &&
                        await IsSosiFile(section))
                    {
                        return await CreateFormFile(contentDisposition, section);
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private static async Task<IFormFile> CreateFormFile(ContentDispositionHeaderValue contentDisposition, MultipartSection section)
        {
            var memoryStream = new MemoryStream();
            await section.Body.CopyToAsync(memoryStream);
            await section.Body.DisposeAsync();
            memoryStream.Position = 0;

            return new FormFile(memoryStream, 0, memoryStream.Length, contentDisposition.Name.ToString(), contentDisposition.FileName.ToString())
            {
                Headers = new HeaderDictionary(),
                ContentType = section.ContentType
            };
        }

        private static async Task<bool> IsSosiFile(MultipartSection section)
        {
            var buffer = new byte[100];
            await section.Body.ReadAsync(buffer.AsMemory(0, 100));
            section.Body.Position = 0;

            using var memoryStream = new MemoryStream(buffer);
            using var streamReader = new StreamReader(memoryStream);
            var sosiString = streamReader.ReadToEnd();

            return sosiString.StartsWith(".HODE");
        }
    }
}
