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
        private string _currentMethodName = string.Empty;
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
        const string pascalCaseClassMethodRegex = "[A-Z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";
        const string camelCaseParameterRegex = "[a-z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";
        const string pascalCaseNamespaceRegex = "[A-Z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";
        const string privatePropertyRegex = "^_[a-z]+[A-Z0-9]+[a-z0-9]*";
        const string publicPropertyRegex = "^[A-Z]+[a-z0-9]+[A-Z0-9]*[a-z0-9]+";

        public void Enter_MethodDeclaration(Method_declarationContext context)
        {
            _currentMethodName = context.method_member_name().GetText();


            var match = Regex.Match(_currentMethodName, pascalCaseClassMethodRegex);
            if (match.Length < _currentMethodName.Length)
            {
                _nameRuleErrorsList.Add(new NameRuleError(NameRuleViolations.MethodNameRuleViolation,
                    _currentNamespace, _currentClassName, _currentMethodName, string.Empty));
            }


            var paramses = context.formal_parameter_list();
            var parameters = string.Empty;
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
                        _nameRuleErrorsList.Add(new NameRuleError(NameRuleViolations.ParameterNameRuleViolation, _currentNamespace, _currentClassName, _currentMethodName, paramName));
                    }
                }
                parameters = _tokenStream.GetText(paramses.Start, paramses.Stop);
            }
            if (_classAlreadyImplementsInterface) { return; }


            var compiledMethod = string.Empty;
            var returnType = string.Empty;
            var accessMod = string.Empty;
            if (context.parent is Typed_member_declarationContext)
            {
                var parent = context.parent as Typed_member_declarationContext;
                var accessModPar = parent.Parent.Parent as Class_member_declarationContext;
                accessMod = accessModPar.Start.Text;
                returnType = _tokenStream.GetText(parent.Start, parent.Start);
                compiledMethod = GetCompiledMethodNameIfPublic(parameters, returnType, accessMod);
            }
            else if (context.parent is Common_member_declarationContext)
            {
                var parent = context.parent as Common_member_declarationContext;
                var accessModPar = parent.Parent.Parent as Class_member_declarationsContext;
                if (accessModPar == null)
                {

                }
                accessMod = accessModPar.Start.Text;
                returnType = _tokenStream.GetText(parent.Start, parent.Start);
                compiledMethod = GetCompiledMethodNameIfPublic(parameters, returnType, accessMod);
            }

            if (!string.IsNullOrEmpty(compiledMethod))
            {
                //generate interface method only if public method
                _interfaceDefinitionBuilder.AppendLine(compiledMethod);
            }
        }

        private string GetCompiledMethodNameIfPublic(string parameters, string returnType, string accessMod)
        {
            string compiledMethod;
            if (accessMod?.ToLower() == "public")
            {
                compiledMethod = string.Format("\t{0} {1} ({2});", returnType, _currentMethodName, parameters);
            }
            else
            {
                compiledMethod = string.Empty;
            }

            return compiledMethod;
        }


        public void Enter_Property_declaration(Property_declarationContext context)
        {
            var propName = context.Start.Text;
            var parent = context.Parent;
            //int anscestorLevel = 4;
            //anscestorLevel > 1 ||
            while (parent.GetType() != typeof(Class_member_declarationsContext))
            {
                parent = parent.Parent;
                //anscestorLevel--;
            }
            var parentInstance = parent as Class_member_declarationsContext;
            var accessMod = parentInstance.Start.Text;

            CheckPropRuleAndUpdateErrorList(propName, accessMod);
        }

        public void Enter_Field_declaration(Field_declarationContext context)
        {
            var fieldName = context.Start.Text;
            var parent = context.Parent;

            while (parent.GetType() != typeof(Class_member_declarationContext))
            {
                parent = parent.Parent;
            }
            var parentInstance = parent as Class_member_declarationContext;
            var accessMod = parentInstance.Start.Text;
            CheckFieldRuleAndUpdateErrorList(fieldName, accessMod);
        }


        private void CheckPropRuleAndUpdateErrorList(string propName, string accessMod)
        {
            string currentRegexString = string.Empty;
            NameRuleViolations violation = NameRuleViolations.Default;
            switch (accessMod.ToLower())
            {
                case "private":
                    //startswith _ and followed by camelCase (_fieldName)
                    currentRegexString = privatePropertyRegex;
                    violation = NameRuleViolations.PrivatePropertyNameRuleViolation;
                    break;
                case "public":
                    violation = NameRuleViolations.PublicPropertyNameRuleViolation;
                    currentRegexString = publicPropertyRegex;
                    break;
                case "protected":
                    //camelCase without _
                    violation = NameRuleViolations.ProtectedPropertyNameRuleViolation;
                    currentRegexString = publicPropertyRegex;
                    break;
                default:
                    break;
            }

            var match = Regex.Match(propName, currentRegexString);
            if (match.Length < propName.Length)
            {
                _nameRuleErrorsList.Add(new NameRuleError(violation, _currentNamespace, _currentClassName, string.Empty, propName));
            }
        }

        private void CheckFieldRuleAndUpdateErrorList(string propOrFieldName, string accessMod)
        {
            string currentRegexString = string.Empty;
            NameRuleViolations violation = NameRuleViolations.Default;
            switch (accessMod.ToLower())
            {
                case "private":
                    //startswith _ and followed by camelCase (_fieldName)
                    currentRegexString = privatePropertyRegex;
                    violation = NameRuleViolations.PrivateFieldNameRuleViolation;
                    break;
                case "public":
                    violation = NameRuleViolations.PublicFieldNameRuleViolation;
                    currentRegexString = publicPropertyRegex;
                    break;
                case "protected":
                    //camelCase without _
                    violation = NameRuleViolations.ProtectedFieldNameRuleViolation;
                    currentRegexString = publicPropertyRegex;
                    break;
                default:
                    break;
            }

            var match = Regex.Match(propOrFieldName, currentRegexString);
            if (match.Length < propOrFieldName.Length)
            {
                _nameRuleErrorsList.Add(new NameRuleError(violation, _currentNamespace, _currentClassName, string.Empty, propOrFieldName));
            }
        }
    }
}
