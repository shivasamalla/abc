using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace RBTCreditControl.Entity
{
    public class UserCorporate
    {
        public int Id { get; set; }

        public int? fk_UserId { get; set; }
        public User User { get; set; }

        public long? fk_CorporateId { get; set; }
        public CorporateMaster CorporateMaster { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

    }
}
