using System.Collections.Generic;
using TruckCheckUp.Core.ViewModels.SituationUI;

namespace TruckCheckUp.Core.Contracts.Services
{
    public interface ISituationService
    {
        SituationViewModel EvaluateSituationDescriptionBeforeAdding(SituationViewModel situationObject);
        void DeleteSituation(string situationId);
        List<SituationListViewModel> RetrieveAllSituations();
        SituationViewModel RetrieveSituationById(string Id);
        SituationViewModel EvaluateSituationDescriptionBeforeSearching(SituationViewModel situationObject);
        SituationViewModel EvaluateSituationDescriptionBeforeUpdating(SituationViewModel situationObject);
    }
}