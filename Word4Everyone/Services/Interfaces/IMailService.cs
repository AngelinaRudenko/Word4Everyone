using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Word4Everyone.Services.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }
}
