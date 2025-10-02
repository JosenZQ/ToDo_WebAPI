using Domain.DTOs;
using Services.Interfaces;

namespace Services.Services
{
    public class EmailContentService : IEmailContentService
    {
        public EmailContentService() { }

        public string GetRegisterEmailBodyContent(RegisterEmailModel pModel)
        {
            try
            {
                string lEmailBody = "<!DOCTYPE html> <html> <head> <meta charset=\"utf-8\" /> <style> body { font-family: Arial; background-color: #f2f2f2; padding: 30px; } .header{ display: flex; align-items: center; flex-direction: column; background-color: white; border-radius: 10px; padding: 5px; margin-bottom: 30px; border: #03045e 2px solid; } .box { border: #03045e 2px solid; background-color: white; padding: 20px; border-radius: 8px; } .container{ display: flex; flex-direction: row; } .container strong, p{ margin-left: 10px; margin-top: 15px; } </style> </head> <body> <div class=\"header\"> <h2>Estimado(a) " + pModel.UserName + "</h2> <p> Su registro se ha completado satisfactoriamente </p> </div> <div class=\"box\"> <p> <strong>IP:</strong> <p>"+ pModel.IPAddress +"</p><br /> <strong>Ubicación:</strong> <p>"+ pModel.Country+", "+ pModel.State+", "+ pModel.City+"</p> <br/> <strong>Fecha y Hora:</strong> <p>"+ pModel.RegisterDate+"</p> <br/> <strong>Zona Horaria:</strong> <p>"+pModel.TimeZoneName+ " "+ pModel.TimeZoneCurrentTime +"</p> </p> </div> </body> </html>";
                return lEmailBody;
            }
            catch(Exception lEx)
            {
                throw lEx;
            }
        }
    }
}
