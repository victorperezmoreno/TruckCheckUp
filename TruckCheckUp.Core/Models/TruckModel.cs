﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class TruckModel : BaseEntity
    {
        [StringLength(30)]
        [DisplayName("Model")]
        [Required]
        public string ModelDescription { get; set; }

        //Link or connection to "join" table
        public virtual ICollection<Truck> Truck { get; set; }

        //Initialize list of Trucks on class creation
        public TruckModel()
        {
            this.Truck = new List<Truck>();
        }
    }
}