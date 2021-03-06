﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class Truck : BaseEntity
    {
        [StringLength(30)]
        [DisplayName("VIN Number")]
        [Required]
        public string VIN { get; set; }

        [DisplayName("Truck Number")]
        [Required]
        public int TruckNumber { get; set; }

        //Indicate whether a truck is active nor inactive
        [Required]
        public bool Status { get; set; }

        //Helps to display a message based on user selection
        public string MessageBasedOnStatusSelection
        {
            get
            {
                return (bool)this.Status ? "Active" : "Inactive";
            }
        }

        [ForeignKey("TruckManufacturer")]
        public string TruckManufacturerId { get; set; }

        [ForeignKey("TruckModel")]
        public string TruckModelId { get; set; }

        [ForeignKey("TruckYear")]
        public string TruckYearId { get; set; }

        // Navigation properties
        public virtual TruckManufacturer TruckManufacturer { get; set; }

        public virtual TruckModel TruckModel { get; set; }

        public virtual TruckYear TruckYear { get; set; }

        //Link or connection to "join" table
        public virtual ICollection<TruckInspection> TruckInspection { get; set; }

        //Initialize list of part reported on class creation
        public Truck()
        {
            this.TruckInspection = new List<TruckInspection>();
        }
    }
}
