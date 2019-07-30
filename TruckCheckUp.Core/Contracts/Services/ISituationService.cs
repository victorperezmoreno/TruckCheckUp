using System.Collections.Generic;
using TruckCheckUp.Core.ViewModels.SituationUI;

namespace TruckCheckUp.Core.Contracts.Services
{
    public interface ISituationService
    {
        SituationViewModel AddSituation(SituationViewModel situationObject);
        void DeleteSituation(string situationId);
        List<SituationListViewModel> RetrieveAllSituations();
        SituationViewModel RetrieveSituationById(string Id);
        SituationViewModel SearchSituation(SituationViewModel situationObject);
        SituationViewModel UpdateSituation(SituationViewModel situationObject);
    }
}