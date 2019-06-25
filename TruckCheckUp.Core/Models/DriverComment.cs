using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckCheckUp.Core.Models
{
    public class DriverComment : BaseEntity
    {
        [StringLength(100)]
        [DisplayName("Details")]
        public string CommentDriver { get; set; }

        [Required]
        [Display(Name = "Ticker Number")]
        public int TicketNumber { get; set; }

    }
}
