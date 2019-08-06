using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.SituationUI
{
    public class SituationListViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        //Indicate whether a situation is active nor inactive
        public bool Status { get; set; } = true;
        
        //Helps to display a message based on user selection
        public string StatusLabel
        {
            get
            {
                return (bool)this.Status ? "Active" : "Inactive";
            }
        }
    }
}
