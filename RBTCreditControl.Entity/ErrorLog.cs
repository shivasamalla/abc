using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RBTCreditControl.Entity
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string ErrorMsg { get; set; }
        public string ErrorDetails { get; set; }
        public string Para { get; set; }
        [MaxLength(500)]
        public string Uri { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
