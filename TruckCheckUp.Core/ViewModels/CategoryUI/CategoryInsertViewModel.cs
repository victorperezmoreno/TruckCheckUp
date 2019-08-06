using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.CategoryUI
{
    public class CategoryInsertViewModel
    {
        [StringLength(30)]
        [DisplayName("Part Category")]
        [Required]
        public string CategoryPart { get; set; }
    }
}
