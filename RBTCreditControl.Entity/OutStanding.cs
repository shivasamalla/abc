using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RBTCreditControl.Entity
{
   public class OutStanding
    {
        [Required]
        public DateTime? fromDate { get; set; }
        [Required]
        public DateTime? toDate { get; set; }
        [Required]
        public decimal? Amount { get; set; }
    }
}
