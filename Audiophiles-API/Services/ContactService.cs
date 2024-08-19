using Audiophiles_API.Data;
using Audiophiles_API.DTOs;
using Audiophiles_API.IServices;
using Audiophiles_API.Models;

namespace Audiophiles_API.Services
{
    public class ContactService: IContactService
    {
        private readonly AudioDbContext _context;
        private readonly IEmailService _emailService;

        public ContactService(AudioDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<RespondDTO> UserContactAsync(UserContactModel model)
        {
            var result = new RespondDTO();

            var newMsg = new UserContact()
            {
                Email = model.Email,
                Title = model.Title,
                Subject = model.Subject,
                Status = false
            };

            // Add New Contact To Database
            _context.UserContacts.Add(newMsg);
            await _context.SaveChangesAsync();

            // Send Notification Email To Admin
            await _emailService.SendEmailAsync("aazzza909.a@gmail.com", newMsg.Title, newMsg.Subject);

            // DTO
            result.IsSucceeded = true;
            result.Message = "Your Message Sent Successfully!";

            return result;
        }

        public async Task<RespondDTO> AdminRespondAsync(AdminRespondModel model)
        {
            var result = new RespondDTO();

            // Check if the contact Id valid
            var userContact = await _context.UserContacts.FindAsync(model.UserContactId);
            if(userContact is null)
            {
                result.IsSucceeded = false;
                result.Message = "Invalid User Contact!!";
                return result;
            }
            // Check if the user contact status is true
            if(userContact.Status == true)
            {
                result.IsSucceeded = false;
                result.Message = "You already responded to this message!!";
                return result;
            }

            // New Admin Respond
            var newRespond = new AdminRespond()
            {
                UserContactId = model.UserContactId,
                Title = model.Title,
                Subject = model.Subject
            };
            // Add To Database
            _context.AdminResponds.Add(newRespond);
            await _context.SaveChangesAsync();
            // Update The User Contact Status
            userContact.Status = true;
            _context.UserContacts.Update(userContact);
            await _context.SaveChangesAsync();

            // Send Notification Email To The User
            await _emailService.SendEmailAsync(userContact.Email, newRespond.Title, newRespond.Subject);

            // DTO
            result.IsSucceeded = true;
            result.Message = "Respond Sent Successfully!";

            return result;
        }

        public List<UserContact> GetAllContactsAsync()
        {
            var contacts = _context.UserContacts.ToList();

            return contacts;
        }
    }
}
