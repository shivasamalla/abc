using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace RBTCreditControl.Entity
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string UserType { get; set; }
        [MaxLength(10)]
        public string EmpCode { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public int? fk_SupervisorId { get; set; }

        [ForeignKey("fk_SupervisorId")]
        public User Supervisor { get; set; }

      

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public ICollection<UserCorporate> lstUserCorporate { get; set; }
        public ICollection<UserLocation> lstUserLocation { get; set; }
        public ICollection<UserCorporateAction> lstUserCorporateAction { get; set; }
        public ICollection<CorporateMaster> LstCorporates { get; set; }

    }
}
