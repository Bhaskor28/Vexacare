using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.Patients.ViewModels;

namespace Vexacare.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterVM model);
        Task<bool> LoginAsync(LoginVM model, bool rememberMe);
        Task LogoutAsync(string userId);
    }
}
