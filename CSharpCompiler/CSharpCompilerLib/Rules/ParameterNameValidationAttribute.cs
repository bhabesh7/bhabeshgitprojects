using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.DataModel;
using System.Text.RegularExpressions;

namespace CSharpCompilerLib.Rules
{
    internal sealed class ParameterNameValidationAttribute : BaseValidationAttribute
    {
        public ParameterNameValidationAttribute()
        {
            ValidationRegexPattern = RegexConstants.CamelCaseParameterRegex;
            NameRuleViolationInstance = NameRuleViolations.ParameterNameRuleViolation;
        }

        public override NameRuleError Validate(string namespaceName, string className, string methodName, string parameterName, string propertyOrFieldName)
        {
            base.Validate(namespaceName, className, methodName, parameterName, propertyOrFieldName);

            return ValidateString(parameterName);
        }
    }
}
