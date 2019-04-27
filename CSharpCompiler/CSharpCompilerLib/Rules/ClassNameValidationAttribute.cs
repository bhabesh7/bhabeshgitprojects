using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.DataModel;

namespace CSharpCompilerLib.Rules
{
    public class ClassNameValidationAttribute : BaseValidationAttribute
    {
        public ClassNameValidationAttribute()
        {
            ValidationRegexPattern = RegexConstants.PascalCaseClassRegex;
            NameRuleViolationInstance = NameRuleViolations.ClassNameRuleViolation;
        }

        public override NameRuleError Validate(string namespaceName, string className, string methodName, string parameterName, string propertyOrFieldName)
        {
            base.Validate(namespaceName, className, methodName, parameterName, propertyOrFieldName);
            return ValidateString(className);
        }
    }
}
