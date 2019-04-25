using CSharpCompilerLib.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using System.Diagnostics;
using Antlr4.Runtime;
using static CSharpCompilerLib.Grammar.CSharpParser;
using CSharpCompilerLib.Rules;
using Accord.DataModel;
using System.Text.RegularExpressions;

namespace CSharpCompilerLib
{
    public class CustomCSharpListener : CSharpParserBaseListener
    {
        StringBuilder _interfacePreProcTemplate = new StringBuilder();

        CSharpParser _parser;
        //StringBuilder _interfaceDefinitionBuilder = new StringBuilder();
        InterfaceExtractorRule _interfaceExtractRule;
        IList<NameRuleError> _nameRuleErrors = new List<NameRuleError>();

        public CustomCSharpListener(CSharpParser parser)
        {
            _parser = parser;
            _interfaceExtractRule = new InterfaceExtractorRule(_parser.InputStream as ITokenStream);
            _interfacePreProcTemplate = new StringBuilder();
        }

        public string GetConvertedInterfaceString()
        {
            var templateNs = _interfacePreProcTemplate.ToString();
            var code = _interfaceExtractRule.GetExtactedInterface();

            //if (string.IsNullOrEmpty(templateNs) && string.IsNullOrEmpty(code))
            //{
            //    return string.Empty;
            //}
            //if (!string.IsNullOrEmpty(templateNs) && string.IsNullOrEmpty(code))
            //{
            //    return string.Empty;
            //}
            //else if(string.IsNullOrEmpty(templateNs) && !string.IsNullOrEmpty(code))
            //{
            //    return code;
            //}
            //else if (!string.IsNullOrEmpty(templateNs) && !string.IsNullOrEmpty(code))
            //{
            //    return (templateNs + "\n" + code + "\n" + "}");
            //}

            return string.IsNullOrEmpty(templateNs) ? code : (templateNs + "\n" + code + "\n" + "}");
        }

        public IList<NameRuleError> GetNameRuleErrorList()
        {
            return _interfaceExtractRule.GetNameRuleErrorList();
            //var comprehensiveList = errList.Union<NameRuleError>(_nameRuleErrors).ToList();
            //return comprehensiveList;
        }

        public override void EnterNamespace_declaration([NotNull] Namespace_declarationContext context)
        {
            base.EnterNamespace_declaration(context);
            _interfaceExtractRule.Enter_NamespaceDefinition(context);

            var id = context.qualified_identifier();
            var tokenStream = _parser.InputStream as ITokenStream;
            var currentNamespace = tokenStream.GetText(id.Start, id.Stop);
            var formattedNs = string.Format("namespace {0}", currentNamespace);
            _interfacePreProcTemplate.AppendLine(formattedNs);
            _interfacePreProcTemplate.Append("{");
        }

        public override void ExitNamespace_declaration([NotNull] Namespace_declarationContext context)
        {
            base.ExitNamespace_declaration(context);
            // _interfacePreProcTemplate.AppendLine("}");
        }

        public override void EnterClass_definition([NotNull] CSharpParser.Class_definitionContext context)
        {
            base.EnterClass_definition(context);
            _interfaceExtractRule.Enter_ClassDefinition(context);
        }

        public override void ExitClass_definition([NotNull] CSharpParser.Class_definitionContext context)
        {
            base.ExitClass_definition(context);

            _interfaceExtractRule.Exit_ClassDefinition(context);
        }

        //public override void EnterClass_member_declaration([NotNull] Class_member_declarationContext context)
        //{
        //    base.EnterClass_member_declaration(context);
        //}

        //public override void EnterBase_type([NotNull] Base_typeContext context)
        //{
        //    base.EnterBase_type(context);
        //}

        //const string privatePropertyRegex = "^_[a-z]+[A-Z0-9]+[a-z0-9]*";
        //const string publicPropertyRegex = "^[A-Z]+[a-z0-9]+[A-Z0-9]*[a-z0-9]+";

        public override void EnterProperty_declaration([NotNull] Property_declarationContext context)
        {
            base.EnterProperty_declaration(context);
            _interfaceExtractRule.Enter_Property_declaration(context);
            
            //var propName = context.Start.Text;
            //var parent = context.Parent;
            ////int anscestorLevel = 4;
            ////anscestorLevel > 1 ||
            //while (parent.GetType() != typeof(Class_member_declarationsContext))
            //{
            //    parent = parent.Parent;
            //    //anscestorLevel--;
            //}

            //var parentInstance = parent as Class_member_declarationsContext;
            //var accessMod = parentInstance.Start.Text;
            //string currentRegexString = string.Empty;
            //switch (accessMod.ToLower())
            //{
            //    case "private":
            //        //startswith _ and followed by camelCase (_fieldName)
            //        currentRegexString = privatePropertyRegex;
            //        break;
            //    case "public":
            //    case "protected":
            //        //PascalCase
            //        currentRegexString = publicPropertyRegex;
            //        //camelCase without _
            //        break;
            //    default:
            //        break;
            //}
            
            //var match = Regex.Match(propName, currentRegexString);
            //if (match.Length < propName.Length)
            //{
            //    _nameRuleErrors.Add(new NameRuleError(NameRuleViolations.PropertyNameRuleViolation, string.Empty, string.Empty, string.Empty, string.Empty));
            //}


        }

        public override void EnterField_declaration([NotNull] Field_declarationContext context)
        {
            base.EnterField_declaration(context);

            _interfaceExtractRule.Enter_Field_declaration(context);
            
        }

        public override void EnterUsingNamespaceDirective([NotNull] UsingNamespaceDirectiveContext context)
        {
            base.EnterUsingNamespaceDirective(context);
            var tokenStream = _parser.InputStream as ITokenStream;
            var text = tokenStream.GetText(context.Start, context.Stop);
            _interfacePreProcTemplate.AppendLine(text);
        }

        public override void EnterMethod_declaration([NotNull] CSharpParser.Method_declarationContext context)
        {
            base.EnterMethod_declaration(context);
            _interfaceExtractRule.Enter_MethodDeclaration(context);
            //var test = context.GetText();

            //var methodName =context.method_member_name().GetText();           
            //var paramses = context.formal_parameter_list();
            //var tokenStream = _parser.InputStream as ITokenStream;
            //var parameters =tokenStream.GetText(paramses.Start, paramses.Stop);
            //var compiledMethod = string.Empty;
            //var returnType = string.Empty;

            //if (context.parent is Typed_member_declarationContext)
            //{
            //    var parent = context.parent as Typed_member_declarationContext;
            //    returnType = tokenStream.GetText(parent.Start, parent.Start);
            //    compiledMethod = string.Format("\t{0} {1} ({2});", returnType, methodName, parameters);
            //}
            //else if (context.parent is Common_member_declarationContext)
            //{
            //    var parent = context.parent as Common_member_declarationContext;
            //    returnType = tokenStream.GetText(parent.Start, parent.Start);
            //    compiledMethod = string.Format("\t{0} {1} ({2});", returnType, methodName, parameters);
            //}            

            //_interfaceDefinitionBuilder.AppendLine(compiledMethod);

        }

        public override void ExitMethod_declaration([NotNull] CSharpParser.Method_declarationContext context)
        {
            base.ExitMethod_declaration(context);
        }

    }
}
