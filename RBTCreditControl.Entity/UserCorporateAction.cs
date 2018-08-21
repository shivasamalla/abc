using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RBTCreditControl.Entity
{
   public class UserCorporateAction
    {
        public long Id { get; set; }

        public long? FK_SubmitedCorporateId { get; set; }
        public SubmitedCorporate SubmitedCorporate { get; set; }

        public int? FK_CorporateStatusMasterId { get; set; }
        public CorporateStatusMaster CorporateStatusMaster { get; set; }

       
        public decimal? PromiseAmount { get; set; }
        public DateTime? PromiseDate { get; set; }

        public decimal?  SbmtAmount { get; set; }
        public DateTime? SbmtFromDate { get; set; }
        public DateTime? SbmtToDate { get; set; }

        public string UniqueGroupId { get; set; }

        [MaxLength(5000)]
        public string CallNote { get; set; }

        public int? UpdatedBy { get; set; }
        public User UserUpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public bool? CurrentStatus { get; set; }

    }
    public class CorporateStatusMaster
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }

        public List<UserCorporateAction> lstUserCorporateAction { get; set; }
    }
}
