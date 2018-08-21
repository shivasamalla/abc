using Newtonsoft.Json;
using RBTCreditControl.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RBTCreditControl.WebApp.Models
{
    public class ERPService
    {
        public List<CorporateMaster> getData(string Location,string SearchPara)
        {
            ErpResp obj = null;
            try
            {
                WebRequest request = WebRequest.Create(string.Format("http://erp.riya.travel/erpvisaservice/services/GetCustomers?CompanyName=Riya%20Travel%20and%20Tours&LocationCode={0}&SearchPattern={1}", Location, SearchPara));
                // request.Credentials = GetCredential();
                request.PreAuthenticate = true;
                NetworkCredential nc = new NetworkCredential();
                nc.Password = "Abc#1898";
                nc.UserName = "visaservice";
                nc.Domain = "RIYACLOUD";
                request.Credentials = nc;
                WebResponse objWebResponse = (WebResponse)request.GetResponse();
                StreamReader objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
                //    // Read the contents.  
                string sResponse = objStreamReader.ReadToEnd();
               obj = JsonConvert.DeserializeObject<ErpResp>(sResponse);
            }
            catch (Exception ex)
            {
                return null;
            }
          
            return obj.Customer;
        }
    }

    //public class ErpCustomer
    //{
    //    public int CustomerType { get; set; }
    //    public string PostingGroup { get; set; }
    //    public string Location { get; set; }
    //    public string No_ { get; set; }
    //    public string Name { get; set; }
    //    public string Phone { get; set; }
    //    public string Email { get; set; }
    //    public int Blocked { get; set; }
    //    public string CustomerGroup { get; set; }
    //}
    public class ErpResp
    {
        public List<CorporateMaster> Customer { get; set; }
    }
}
