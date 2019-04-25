using Accord.DataModel;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static CSharpCompilerLib.Grammar.CSharpParser;

namespace CSharpCompilerLib.Rules
{
    public class InterfaceExtractorRule
    {
        ITokenStream _tokenStream;
        StringBuilder _interfaceDefinitionBuilder = new StringBuilder();
        IList<NameRuleError> _nameRuleErrorsList = new List<NameRuleError>();
        private string _currentClassName = string.Empty;
        private string _currentNamespace = string.Empty;
        private bool _classAlreadyImplementsInterface = false;

        public InterfaceExtractorRule(ITokenStream tokenStream)
        {
            _tokenStream = tokenStream;
        }

        public string GetExtactedInterface()
        {
            return _interfaceDefinitionBuilder.ToString();
        }

        public IList<NameRuleError> GetNameRuleErrorList()
        {
            return _nameRuleErrorsList;
        }


        public void Enter_NamespaceDefinition(Namespace_declarationContext context)
        {
            var id = context.qualified_identifier();
            _currentNamespace = _tokenStream.GetText(id.Start, id.Stop);
            if (string.IsNullOrEmpty(_currentNamespace))
            {
                return;
            }
            var match = Regex.Match(_currentNamespace, pascalCaseNamespaceRegex);
            if (match.Length < _currentNamespace.Length)
            {
                _nameRuleErrorsList.Add(new NameRuleError(NameRuleViolations.NamespaceRuleViolation, _currentNamespace, string.Empty, string.Empty, string.Empty));
            }
        }

        public void Enter_ClassDefinition(Class_definitionContext context)
        {
            var id = context.identifier();
            _currentClassName = _tokenStream.GetText(id.Start, id.Stop);

            var match = Regex.Match(_currentClassName, pascalCaseClassMethodRegex);
            if (match.Length < _currentClassName.Length)
            {
                _nameRuleErrorsList.Add(new NameRuleError(NameRuleViolations.ClassNameRuleViolation, _currentNamespace, _currentClassName, string.Empty, string.Empty));
            }

            var tpcc = context.type_parameter_constraints_clauses();
            var tpl = context.type_parameter_list();
            if (tpl != null)
            {
                var gen = _tokenStream.GetText(tpl.Start, tpl.Stop);
            }

            if (context.children.Count >= 4)
            {
                //Class has Implemented Interfaces
                Class_baseContext interfaceImpl = (Class_baseContext)context.children.FirstOrDefault((x) => x.GetType() == typeof(Class_baseContext));
                if (interfaceImpl?.Start?.Text == ":")
                {
                    //This implements one or multiple interfaces
                    _classAlreadyImplementsInterface = true;
                }
            }

            if (_classAlreadyImplementsInterface)
            { return; }
            //var parent = context.Parent as Common_member_declarationContext;
            //var accessMod = ts.GetText(parent.Start, parent.Start);
            var interfaceName = string.Format("{0}{1}", "I", _currentClassName);
            var compiledInterfaceName = string.Format("public interface {0}", interfaceName);
            _interfaceDefinitionBuilder.AppendLine();
            _interfaceDefinitionBuilder.Append(compiledInterfaceName);
            _interfaceDefinitionBuilder.AppendLine();
            _interfaceDefinitionBuilder.Append("{");
        }


        public void Exit_ClassDefinition(Class_definitionContext context)
        {
            if (_classAlreadyImplementsInterface) { return; }
            _interfaceDefinitionBuilder.Append("}");

        }
        string pascalCaseClassMethodRegex = "[A-Z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";
        string camelCaseParameterRegex = "[a-z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";
        string pascalCaseNamespaceRegex = "[A-Z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";


        public void Enter_MethodDeclaration(Method_declarationContext context)
        {
            var methodName = context.method_member_name().GetText();

            var match = Regex.Match(methodName, pascalCaseClassMethodRegex);
            if (match.Length < methodName.Length)
            {
                _nameRuleErrorsList.Add(new NameRuleError(NameRuleViolations.MethodNameRuleViolation, string.Empty, _currentClassName, methodName, string.Empty));
            }


            var paramses = context.formal_parameter_list();

            if (paramses != null)
            {
                //Method parameters detected
                var paramArgs = paramses.fixed_parameters();
                for (int i = 0; i < paramArgs?.children.Count; i++)
                {
                    if (i % 2 != 0)
                    {
                        continue;
                    }
                    var p = paramArgs.children[i] as Fixed_parameterContext;
                    var paramName = p.Stop.Text;

                    var matchP = Regex.Match(paramName, camelCaseParameterRegex);
                    if (matchP.Length < paramName.Length)
                    {
                        _nameRuleErrorsList.Add(new NameRuleError(NameRuleViolations.ParameterNameRuleViolation, _currentNamespace, _currentClassName, methodName, paramName));
                    }
                }
            }
            if (_classAlreadyImplementsInterface) { return; }

            var parameters = _tokenStream.GetText(paramses.Start, paramses.Stop);
            var compiledMethod = string.Empty;
            var returnType = string.Empty;

            if (context.parent is Typed_member_declarationContext)
            {
                var parent = context.parent as Typed_member_declarationContext;
                returnType = _tokenStream.GetText(parent.Start, parent.Start);
                compiledMethod = string.Format("\t{0} {1} ({2});", returnType, methodName, parameters);
            }
            else if (context.parent is Common_member_declarationContext)
            {
                var parent = context.parent as Common_member_declarationContext;
                returnType = _tokenStream.GetText(parent.Start, parent.Start);
                compiledMethod = string.Format("\t{0} {1} ({2});", returnType, methodName, parameters);
            }

            _interfaceDefinitionBuilder.AppendLine(compiledMethod);
        }


    }
}
