using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.DataModel;

namespace CSharpCompilerLib.Rules
{
    internal sealed class MaxLengthValidationAttribute : BaseValidationAttribute
    {
        public MaxLengthValidationAttribute(int maxLength, NameRuleViolations violationType) : base(maxLength, violationType)
        {

        }

        public override NameRuleError Validate(string namespaceName, string className, string methodName, string parameterName, string propertyOrFieldName)
        {
            base.Validate(namespaceName, className, methodName, parameterName, propertyOrFieldName);
            string item = string.Empty;
            switch (NameRuleViolationInstance)
            {
                case NameRuleViolations.Default:
                    break;
                case NameRuleViolations.MethodNameRuleViolation:
                    return ValidateString(methodName);

                case NameRuleViolations.ParameterNameRuleViolation:
                    return ValidateString(parameterName);

                case NameRuleViolations.ClassNameRuleViolation:
                    return ValidateString(className);

                case NameRuleViolations.NamespaceRuleViolation:
                    return ValidateString(namespaceName);

                case NameRuleViolations.PublicFieldNameRuleViolation:
                case NameRuleViolations.ProtectedFieldNameRuleViolation:
                case NameRuleViolations.PrivateFieldNameRuleViolation:
                case NameRuleViolations.PropertyNameRuleViolation:
                case NameRuleViolations.PublicPropertyNameRuleViolation:
                case NameRuleViolations.ProtectedPropertyNameRuleViolation:
                case NameRuleViolations.PrivatePropertyNameRuleViolation:
                    return ValidateString(propertyOrFieldName);
                default:
                    break;
            }
            return default(NameRuleError);
        }

        /// <summary>
        /// Check the MaxLength rule here for all Types
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override NameRuleError ValidateString(string item)
        {
            if(item.Length > MaxLenth)
            {
                return new NameRuleError(NameRuleViolations.NameLengthExceededRuleViolation, _currentNamespaceName, _className, _currentMethodName, _parameterName, _propertyOrFieldName);
            }
            return default(NameRuleError);
        }

    }
}
