using Project_Login.ViewModels;

namespace Project_Login.Interfaces;

public interface IAuthService
{

    Task<LoginResponseViewModel> GerarJwt(string loginUserEmail);
    
}
