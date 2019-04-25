using Accord.Interfaces;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CSharpCompilerLib.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Accord.DataModel;

namespace CSharpCompilerLib
{
    //Accord
    [Export(typeof(IAnalysisManager))]
    public class AnalysisManager : IAnalysisManager
    {
        public AnalysisResultData RunAnalysis(string programString)
        {
            AntlrInputStream inputStream = new AntlrInputStream(programString);
            CSharpLexer lexer = new CSharpLexer(inputStream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);

            CSharpParser parser = new CSharpParser(tokenStream);
            

            //Grammar.CSharpParserBaseVisitor<Data> visitor = new CSharpParserBaseVisitor<Data>();
            //ctx.Accept<Data>(visitor);
            CustomCSharpListener listener = new CustomCSharpListener(parser);
            ParseTreeWalker walker = new ParseTreeWalker();
            var ctx = parser.using_directives();
            walker.Walk(listener, ctx);

            var ctx1 = parser.namespace_member_declaration();
            walker.Walk(listener, ctx1);

            //var ctx2 = parser.class_member_declaration();
            //walker.Walk(listener, ctx1);

            AnalysisResultData analysisResData = new AnalysisResultData();

            var errors = listener.GetNameRuleErrorList();
            foreach (var err in errors)
            {
                analysisResData.NameRuleErrors.Add(err);
            }
            analysisResData.GeneratedInterfaceString = listener.GetConvertedInterfaceString();

            //if (string.IsNullOrEmpty(analysisResData.GeneratedInterfaceString) || analysisResData.NameRuleErrors.Count == 0)
            //{
            //    var ctx1 = parser.namespace_member_declaration();
            //    walker.Walk(listener, ctx1);
            //    errors = listener.GetNameRuleErrorList();
            //    foreach (var err in errors)
            //    {
            //        analysisResData.NameRuleErrors.Add(err);
            //    }
            //}


            return analysisResData;

        }
    }
}
