using System;
using System.Collections.Generic;
using System.Text;

namespace RBTCreditControl.Entity
{
    public class UserLocation
    {
        public int Id { get; set; }

        public int FK_LocationId { get; set; }
        public LocationMaster Location { get; set; }

        public int FK_UserId { get; set; }
        public User User { get; set; }
    }
}
