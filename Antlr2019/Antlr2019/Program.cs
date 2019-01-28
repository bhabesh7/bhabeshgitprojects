 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using System.IO;
using Antlr2019.Model;
using Antlr4.Runtime.Tree;

namespace Antlr2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("FL_insurance_sample.csv");
            //IList<InsurancePolicyData> insurancePolicyDataList = new List<InsurancePolicyData>();

            for (int i = 0; i < lines.Length/1000; i++)
            {
                if(i == 0)
                { continue; }

                AntlrInputStream inputStream = new AntlrInputStream(lines[i]);
                InsurancePolicyRulesLexer lexer = new InsurancePolicyRulesLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);

                InsurancePolicyRulesParser parser = new InsurancePolicyRulesParser(commonTokenStream);
                var ctx = parser.csvFile();

                InsurancePolicyRulesBaseVisitor<InsurancePolicyData> vis = new InsurancePolicyRulesBaseVisitor<InsurancePolicyData>();
                InsurancePolicyCustomListener customListener = new InsurancePolicyCustomListener();
                ParseTreeWalker parseTreeWalker = new ParseTreeWalker();
                parseTreeWalker.Walk(customListener, ctx);
                var data = customListener.GetInsurancePolicyData();
                
                string formattedData = string.Format("[{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7}]", 
                    data.PolicyID, data.StateCode, data.EqSiteLimit, data.HuSiteLimit, data.FlSiteLimit, data.FrSiteLimit,
                    data.Line, data.Construction);
                Console.WriteLine(formattedData);
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

        }
    }
}
