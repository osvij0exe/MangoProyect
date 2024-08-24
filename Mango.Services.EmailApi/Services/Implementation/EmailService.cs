using Mango.Services.EmailApi.Data;
using Mango.Services.EmailApi.Data.Dtos;
using Mango.Services.EmailApi.Message;
using Mango.Services.EmailApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailApi.Services.Implementation
{
    
    public class EmailService : IEmailService
    {
        private  DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }
        public async Task EmailCartAndLog(CartDto cartDto)
        {
            //template para el email a enviar

            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Request");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>"); 
            message.Append("<ul/>");
            foreach(var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.ProductDto.Name + " x " + item.Count);
                message.Append("</li>");
            }

            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);

        }

        public async Task LogOrderPlaced(RewardsMessage rewardsDto)
        {
            string message = "New Order Placed. <br/> Order ID: " + rewardsDto.OrderId;
            await LogAndEmail(message, "dotnetmastery@gmail.com");
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = "User Registration Successful. <br/> Email: " + email;
            await LogAndEmail(message, "dotnetmastery@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message,string email)
        {
            try
            {

                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();


                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
