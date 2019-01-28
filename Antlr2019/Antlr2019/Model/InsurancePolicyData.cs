using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antlr2019.Model
{
    public class InsurancePolicyData
    {
        public int PolicyID { get; set; }
        public string StateCode { get; set; }
        public string Country { get; set; }
        public double EqSiteLimit { get; set; }
        public double HuSiteLimit { get; set; }
        public double FlSiteLimit { get; set; }
        public double FrSiteLimit { get; set; }
        public double Tiv2011 { get; set; }
        public double Tiv2012 { get; set; }
        public double EqSiteDeductible { get; set; }
        public double HuSiteDeductible { get; set; }
        public double FlSiteDeductible { get; set; }
        public double FrSiteDeductible { get; set; }
        public double PointLatitude { get; set; }
        public double PointLongitude { get; set; }
        public string Line { get; set; }
        public string Construction { get; set; }
        public int PointGranularity { get; set; }








    }
}
