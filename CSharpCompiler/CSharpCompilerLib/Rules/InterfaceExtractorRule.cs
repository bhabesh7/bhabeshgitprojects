using Accord.DataModel;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        StringBuilder _interfacePreProcTemplate = new StringBuilder();

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
            var templateNs = _interfacePreProcTemplate.ToString();
            var code = _interfaceDefinitionBuilder.ToString();
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
            return _nameRuleErrorsList;
        }


        public void EnterUsingNamespaceDirective(UsingNamespaceDirectiveContext context)
        {                        
            var text = _tokenStream.GetText(context.Start, context.Stop);
            _interfacePreProcTemplate.AppendLine(text);
        }


        public void Enter_Namespace_declaration(Namespace_declarationContext context)
        {
            var id = context.qualified_identifier();
            _currentNamespace = _tokenStream.GetText(id.Start, id.Stop);
            if (string.IsNullOrEmpty(_currentNamespace))
            {
                return;
            }
            var match = Regex.Match(_currentNamespace, RegexConstants.PascalCaseNamespaceRegex);
            if (match.Length < _currentNamespace.Length)
            {
                _nameRuleErrorsList.Add(new NameRuleError(NameRuleViolations.NamespaceRuleViolation, _currentNamespace, string.Empty, string.Empty, string.Empty));
            }            
            
            var currentNamespace = _tokenStream.GetText(id.Start, id.Stop);
            var formattedNs = string.Format("namespace {0}", currentNamespace);
            _interfacePreProcTemplate.AppendLine(formattedNs);
            _interfacePreProcTemplate.Append("{");
        }

        [ClassNameValidationAttribute()]
        public void Enter_ClassDefinition(Class_definitionContext context)
        {
            var id = context.identifier();
            _currentClassName = _tokenStream.GetText(id.Start, id.Stop);

            var classRuleAttr = ValidationRuleProvider<ClassNameValidationAttribute>(nameof(Enter_ClassDefinition)) as ClassNameValidationAttribute;
            
            if (classRuleAttr != null)
            {
                var nameRuleError = classRuleAttr.Validate(_currentNamespace, _currentClassName, string.Empty, string.Empty, string.Empty);
                if (nameRuleError != null)
                {
                    _nameRuleErrorsList.Add(nameRuleError);
                }
            }

            //var match = Regex.Match(_currentClassName, RegexConstants.PascalCaseClassMethodRegex);
            //if (match.Length < _currentClassName.Length)
            //{
            //    _nameRuleErrorsList.Add(new NameRuleError(NameRuleViolations.ClassNameRuleViolation, _currentNamespace, _currentClassName, string.Empty, string.Empty));
            //}

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
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private BaseValidationAttribute ValidationRuleProvider<T>(string methodName) where T : BaseValidationAttribute
        {
            var currentMethod = typeof(InterfaceExtractorRule).GetMethod(methodName);
            //var currentMethod = MethodBase.GetCurrentMethod(); //slower meth apparently
            if (currentMethod == null) { return null; }
            var ruleAttr = currentMethod.GetCustomAttributes(false).OfType<T>().FirstOrDefault();
            return ruleAttr;
        }

        [MethodNameValidation()]     
        [MaxLengthValidation(50, NameRuleViolations.MethodNameRuleViolation)]
        public void Enter_MethodDeclaration(Method_declarationContext context)
        {
            _currentMethodName = context.method_member_name().GetText();

            var ruleAttr = ValidationRuleProvider<MethodNameValidationAttribute>(nameof(Enter_MethodDeclaration)) as MethodNameValidationAttribute;
            if (ruleAttr != null)
            {
                var nameRuleError = ruleAttr.Validate(_currentNamespace, _currentClassName, _currentMethodName, string.Empty, string.Empty);
                if (nameRuleError != null)
                {
                    _nameRuleErrorsList.Add(nameRuleError);
                }
            }


            var maxLenAttr = ValidationRuleProvider<MaxLengthValidationAttribute>(nameof(Enter_MethodDeclaration)) as MaxLengthValidationAttribute;
            if (maxLenAttr != null)
            {
                var nameRuleError = maxLenAttr.Validate(_currentNamespace, _currentClassName, _currentMethodName, string.Empty, string.Empty);
                if (nameRuleError != null)
                {
                    _nameRuleErrorsList.Add(nameRuleError);
                }
            }

            var paramses = context.formal_parameter_list();
            string parameters = HandleMethodParameters(paramses);
            ExtractInterfaceMethodSignature(context, parameters);
        }

        /// <summary>
        /// Detects and validates parameters
        /// </summary>
        /// <param name="paramses"></param>
        /// <returns></returns>
        [ParameterNameValidation()]
        private string HandleMethodParameters(Formal_parameter_listContext paramses)
        {
            var parameters = string.Empty;
            if (paramses != null)
            {
                //Method parameters detected
                var paramArgs = paramses.fixed_parameters();
                var paramRuleAttr = ValidationRuleProvider<ParameterNameValidationAttribute>(nameof(HandleMethodParameters)) as ParameterNameValidationAttribute;

                for (int i = 0; i < paramArgs?.children.Count; i++)
                {
                    if (i % 2 != 0)
                    {
                        continue;
                    }
                    var p = paramArgs.children[i] as Fixed_parameterContext;
                    var paramName = p.Stop.Text;

                    if (paramRuleAttr != null)
                    {
                        var paramNameRuleError = paramRuleAttr.Validate(_currentNamespace, _currentClassName, _currentMethodName, paramName, string.Empty);
                        if (paramNameRuleError != null)
                        {
                            _nameRuleErrorsList.Add(paramNameRuleError);
                        }
                    }
                }
                parameters = _tokenStream.GetText(paramses.Start, paramses.Stop);
            }

            return parameters;
        }

        /// <summary>
        /// Extracts interface from a concrete C# class post method inspection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        private void ExtractInterfaceMethodSignature(Method_declarationContext context, string parameters)
        {
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

        /// <summary>
        /// Check Prop rule
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="accessMod"></param>
        [PublicPropertyValidation()]
        [ProtectedPropertyValidation()]
        [PrivatePropertyValidation()]
        private void CheckPropRuleAndUpdateErrorList(string propName, string accessMod)
        {
            string currentRegexString = string.Empty;
            NameRuleViolations violation = NameRuleViolations.Default;
            switch (accessMod.ToLower())
            {
                case "private":
                    //startswith _ and followed by camelCase (_fieldName)
                    //currentRegexString = RegexConstants.PrivatePropertyRegex;
                    //violation = NameRuleViolations.PrivatePropertyNameRuleViolation;
                    var privRule = ValidationRuleProvider<PrivatePropertyValidationAttribute>(nameof(CheckPropRuleAndUpdateErrorList)) as PrivatePropertyValidationAttribute;
                    if(privRule !=null)
                    {
                        privRule.Validate(_currentNamespace, _currentClassName, _currentMethodName, string.Empty, propName);
                    }

                    break;
                case "public":
                    //violation = NameRuleViolations.PublicPropertyNameRuleViolation;
                    //currentRegexString = RegexConstants.PublicPropertyRegex;
                    var pubRule = ValidationRuleProvider<PublicPropertyValidationAttribute>(nameof(CheckPropRuleAndUpdateErrorList)) as PublicPropertyValidationAttribute;
                    if (pubRule != null)
                    {
                        pubRule.Validate(_currentNamespace, _currentClassName, _currentMethodName, string.Empty, propName);
                    }
                    break;
                case "protected":
                    //camelCase without _
                    //violation = NameRuleViolations.ProtectedPropertyNameRuleViolation;
                    //currentRegexString = RegexConstants.PublicPropertyRegex;

                    var protRule = ValidationRuleProvider<ProtectedPropertyValidationAttribute>(nameof(CheckPropRuleAndUpdateErrorList)) as ProtectedPropertyValidationAttribute;
                    if (protRule != null)
                    {
                        protRule.Validate(_currentNamespace, _currentClassName, _currentMethodName, string.Empty, propName);
                    }
                    break;
                default:
                    break;
            }

            //var match = Regex.Match(propName, currentRegexString);
            //if (match.Length < propName.Length)
            //{
            //    _nameRuleErrorsList?.Add(new NameRuleError(violation, _currentNamespace, _currentClassName, string.Empty, propName));
            //}
        }

        /// <summary>
        /// Check Field Rule
        /// </summary>
        /// <param name="propOrFieldName"></param>
        /// <param name="accessMod"></param>
        private void CheckFieldRuleAndUpdateErrorList(string propOrFieldName, string accessMod)
        {
            string currentRegexString = string.Empty;
            NameRuleViolations violation = NameRuleViolations.Default;
            switch (accessMod.ToLower())
            {
                case "private":
                    //startswith _ and followed by camelCase (_fieldName)
                    currentRegexString = RegexConstants.PrivatePropertyRegex;
                    violation = NameRuleViolations.PrivateFieldNameRuleViolation;
                    break;
                case "public":
                    violation = NameRuleViolations.PublicFieldNameRuleViolation;
                    currentRegexString = RegexConstants.PublicPropertyRegex;
                    break;
                case "protected":
                    //camelCase without _
                    violation = NameRuleViolations.ProtectedFieldNameRuleViolation;
                    currentRegexString = RegexConstants.PublicPropertyRegex;
                    break;
                default:
                    break;
            }

            var match = Regex.Match(propOrFieldName, currentRegexString);
            if (match.Length < propOrFieldName.Length)
            {
                _nameRuleErrorsList?.Add(new NameRuleError(violation, _currentNamespace, _currentClassName, string.Empty, propOrFieldName));
            }
        }

        public void EnterMethod_body(Method_bodyContext context)
        {
            //base.EnterMethod_body(context);
            var body = _tokenStream.GetText(context.Start, context.Stop);
            var lines = body.Split('\n').Length;
            if (lines > 50)
            {
                _nameRuleErrorsList?.Add(new NameRuleError(NameRuleViolations.LargeMethodBodyRuleViolation, _currentNamespace, _currentClassName, _currentMethodName, string.Empty));
            }

        }
    }
}
