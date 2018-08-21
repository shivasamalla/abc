using RBTCreditControl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RBTCreditControl.Repository
{
   public interface ILocationMaster_Repo
    {
        void Save(LocationMaster obj);
        IEnumerable<LocationMaster> GetALL();
        IEnumerable<LocationMaster> GetALL(int page, int pagesize);
    }
   public class LocationMaster_Repo: ILocationMaster_Repo
    {
        private RBTCreditControl_Context _Context = null;
        public LocationMaster_Repo(RBTCreditControl_Context Context)
        {
            _Context = Context;
        }

        public IEnumerable<LocationMaster> GetALL()
        {
            var resp =  _Context.LocationMaster.ToList();
            return resp;
        }
        public IEnumerable<LocationMaster> GetALL(int page, int pagesize)
        {
            var resp = _Context.LocationMaster.GetPaged(page, pagesize).Results.Cast<LocationMaster>(); 
            return resp;
        }
        public void Save(LocationMaster obj)
        {
            _Context.Set<LocationMaster>().Add(obj);
            _Context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
