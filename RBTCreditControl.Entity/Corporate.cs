using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace RBTCreditControl.Entity
{
    public class CorporateMaster
    {
        public long Id { get; set; }

        [MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(200)]
        public string Location { get; set; }
        [MaxLength(200)]
        public string No_ { get; set; }
        [MaxLength(200)]
        public string Email { get; set; }
        [MaxLength(200)]
        public string PostingGroup { get; set; }
        [MaxLength(200)]
        public string CustomerGroup { get; set; }
        [MaxLength(200)]
        public string CustomerType { get; set; }
        [MaxLength(200)]
        public string Blocked { get; set; }

        public decimal? Balance { get; set; }

        public bool UpdateFlag { get; set; }

        public int Priority { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public DateTime? PrioriyUpdatedOn { get; set; }
        public int? PriorityUpdatedBy { get; set; }

        public DateTime? OstSubmittedOn { get; set; }
        //public int? OstSubmittedBy { get; set; }

        public int fk_LocationId { get; set; }
        public LocationMaster LocationMaster { get; set; }

        public ICollection<UserCorporate> lstUserCorporate { get; set; }
        public ICollection<SubmitedCorporate> lstSubmitedCorporate { get; set; }
    }
    public class SubmitedCorporate
    {
        public long Id { get; set; }

        public decimal? Balance { get; set; }

        public long? FK_CorporateMasterId { get; set; }
        public CorporateMaster CorporateMaster { get; set; }
        public List<UserCorporateAction> lstUserCorporateAction { get; set; }
    }
}
