using System;
using System.Collections.Generic;
using System.Text;

namespace RBTCreditControl.Entity
{
  public enum CorporateStatus{
        Promise=1, CreditNoteAwaited=2, InvoiceNotSubmitted=3, AttachmentMissing=4, WrongBilling=5, Reconciliation=6, Others=7, Received=8, Submition=9
    }

    public class UserReportSearchPara
    {
        public DateTime dtFromDate { get; set; }
        public DateTime dtToDate { get; set; }
        public int statusId { get; set; }
        public string ICUST { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int userId { get; set; }
    }
}
