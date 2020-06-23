using TransferService.Domain;
using TransferService.Infra.CrossCutting.Identity;

namespace TransferService.Infra.CrossCutting.Identity.Interfaces
{
    public interface IApplicationSignInManager
    {
        object GenerateTokenAndSetIdentity(User user, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations);
    }
}
