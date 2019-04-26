using Accord.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCompilerLib.Rules
{    
    internal sealed class ProtectedPropertyValidationAttribute : BaseValidationAttribute
    {
        public ProtectedPropertyValidationAttribute() : base()
        {
            ValidationRegexPattern = RegexConstants.PublicPropertyRegex;
            NameRuleViolationInstance = NameRuleViolations.ProtectedPropertyNameRuleViolation;
        }

        public override NameRuleError Validate(string namespaceName, string className, string methodName, string parameterName, string propertyOrFieldName)
        {
            base.Validate(namespaceName, className, methodName, parameterName, propertyOrFieldName);
            return ValidateString(propertyOrFieldName);
        }

    }
}
