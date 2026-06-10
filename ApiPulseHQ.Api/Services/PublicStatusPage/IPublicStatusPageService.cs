using ApiPulseHQ.Api.Models.PublicStatusPage;

namespace ApiPulseHQ.Api.Services.PublicStatusPage
{
    public interface IPublicStatusPageService
    {
        Task<PublicStatusPageResponse?> GetBySlugAsync(string slug);
    }
}
