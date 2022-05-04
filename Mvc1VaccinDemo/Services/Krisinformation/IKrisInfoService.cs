using System.Collections.Generic;

namespace Mvc1VaccinDemo.Services.Krisinformation
{
    public interface IKrisInfoService
    {
        List<KrisInfo> GetAllKrisInformation();
        List<KrisInfo> GetEmergencies();
        KrisInfo GetKrisInformation(string id);
    }
}