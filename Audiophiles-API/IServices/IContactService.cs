using Audiophiles_API.DTOs;
using Audiophiles_API.Models;

namespace Audiophiles_API.IServices
{
    public interface IContactService
    {
        Task<RespondDTO> UserContactAsync(UserContactModel model);
        Task<RespondDTO> AdminRespondAsync(AdminRespondModel model);
        UserContact GetContactAsync(int id);
        List<UserContact> GetAllContactsAsync();
    }
}
