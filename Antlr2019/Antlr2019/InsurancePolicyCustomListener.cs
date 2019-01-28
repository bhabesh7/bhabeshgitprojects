using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr2019.Model;

namespace Antlr2019
{
    public class InsurancePolicyCustomListener: InsurancePolicyRulesBaseListener
    {
        //List<InsurancePolicyData> _insurancePolicyDataList = new List<InsurancePolicyData>();
        private InsurancePolicyData _insurancePolicyDataInstance { get; set; }
        

        public InsurancePolicyData GetInsurancePolicyData()
        {
            return _insurancePolicyDataInstance;
        }

        public override void EnterRow([NotNull] InsurancePolicyRulesParser.RowContext context)
        {
            base.EnterRow(context);
            if(context.start.Text=="<EOF>")
            {
                return;
            }
            _insurancePolicyDataInstance = new InsurancePolicyData();
        }
        public override void EnterField([NotNull] InsurancePolicyRulesParser.FieldContext context)
        {
            base.EnterField(context);

            switch (context.Start.TokenIndex)
            {
                case 0:
                    int policyID = 0;
                    if(int.TryParse(context.Start.Text ,out policyID))
                    {
                        _insurancePolicyDataInstance.PolicyID = policyID;
                    }
                    break;
                case 2:
                    _insurancePolicyDataInstance.StateCode = context.Start.Text;
                    break;
                case 4:
                    _insurancePolicyDataInstance.Country = context.Start.Text;
                    break;
                case 6:
                    double eqSiteLimit = 0;
                    if (double.TryParse(context.Start.Text, out eqSiteLimit))
                    {
                        _insurancePolicyDataInstance.EqSiteLimit = eqSiteLimit;
                    }
                    break;
                case 8:
                    double huSiteLimit = 0;
                    if (double.TryParse(context.Start.Text, out huSiteLimit))
                    {
                        _insurancePolicyDataInstance.HuSiteLimit = huSiteLimit;
                    }
                    break;
                case 10:
                    double flSiteLimit = 0;
                    if (double.TryParse(context.Start.Text, out flSiteLimit))
                    {
                        _insurancePolicyDataInstance.FlSiteLimit = flSiteLimit;
                    }
                    break;
                case 30:
                    _insurancePolicyDataInstance.Line = context.Start.Text;
                    break;
                case 32:
                    _insurancePolicyDataInstance.Construction = context.Start.Text;
                    break;
                default:
                    break;
            }

        }
        public override void ExitField([NotNull] InsurancePolicyRulesParser.FieldContext context)
        {
            base.ExitField(context);
        }
    }
}
