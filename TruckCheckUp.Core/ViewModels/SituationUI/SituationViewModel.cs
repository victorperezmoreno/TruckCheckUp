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

        public string Description { get; set; }

        public bool Status { get; set; } = true;

        public bool ExistInDB { get; set; } = true;

        public bool IsValid { get; set; } = true;

        public string StatusLabel
        {
            get
            {
                return (bool)this.Status ? "Active" : "Inactive";
            }
        }
    }
}
