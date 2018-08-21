using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RBTCreditControl.Entity
{
    public class LocationMaster
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Branch { get; set; }

        [MaxLength(500)]
        public string BranchLocation { get; set; }

        [MaxLength(500)]
        public string Dimension { get; set; }

        [MaxLength(500)]
        public string CityName { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public ICollection<CorporateMaster> lstCorporateMaster { get; set; }

        public ICollection<UserLocation> lstUserLocation { get; set; }
        //public ICollection<LocationCorporate> lstLocationCorporate { get; set; }
    }
}
