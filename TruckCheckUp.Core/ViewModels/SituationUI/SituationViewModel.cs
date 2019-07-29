using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.SituationUI
{
    public class SituationViewModel
    {
        public string Id { get; set; }

        public string Situation { get; set; }

        public bool ExistInDB { get; set; } = true;

        public bool IsValid { get; set; } = true;

        public bool Status { get; set; } = true;
    }
}
