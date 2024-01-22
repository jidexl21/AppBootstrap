using AppBootstrap.Core;

namespace AppBootstrap.Services.Interfaces
{
    public interface ICaptainsService
    {
        Task<Captain> GetMyFaveCaptain(long location);
    }
}