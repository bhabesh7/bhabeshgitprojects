using Accord.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpCompilerLib.Rules
{
    internal sealed class MethodNameValidationAttribute : BaseValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationRegex">Provide the Regex upon which validation will take place</param>
        public MethodNameValidationAttribute()
        {
            ValidationRegexPattern = RegexConstants.PascalCaseClassMethodRegex;            
            NameRuleViolationInstance = NameRuleViolations.MethodNameRuleViolation;
        }        
        
        public override NameRuleError Validate(string namespaceName, string className, string methodName, string parameterName, string propertyOrFieldName)
        {
            base.Validate(namespaceName, className, methodName, parameterName, propertyOrFieldName);
            return ValidateString(methodName);
        }
    }
}
