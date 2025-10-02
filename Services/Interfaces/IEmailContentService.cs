using Domain.DTOs;

namespace Services.Interfaces
{
    public interface IEmailContentService
    {
        string GetRegisterEmailBodyContent(RegisterEmailModel pModel);
    }
}
