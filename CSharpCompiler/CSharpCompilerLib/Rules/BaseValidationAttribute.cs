using Accord.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpCompilerLib.Rules
{
    public class BaseValidationAttribute: Attribute
    {
        protected string _className;
        protected string _currentNamespaceName;
        protected string _currentMethodName;
        protected string _parameterName;
        protected string _propertyOrFieldName;
        
        protected int MaxLenth { get; set; }
        protected string ValidationRegexPattern { get; set; }
        protected NameRuleViolations NameRuleViolationInstance { get; set; }

        public BaseValidationAttribute()
        {            
            NameRuleViolationInstance = NameRuleViolations.Default;
        }
               
        /// <summary>
        /// Ctor only to be used when doing Length validation
        /// </summary>
        /// <param name="maxLength"></param>
        public BaseValidationAttribute(int maxLength, NameRuleViolations violationType)
        {
            MaxLenth = maxLength;
            NameRuleViolationInstance = violationType;
        }

        public virtual NameRuleError Validate(string namespaceName, string className, string methodName, string parameterName, string propertyOrFieldName)
        {
            _currentNamespaceName = namespaceName;
            _className = className;
            _currentMethodName = methodName;
            _parameterName = parameterName;
            _propertyOrFieldName = propertyOrFieldName;
            return default(NameRuleError);
        }

        protected virtual NameRuleError ValidateString(string item)
        {
            var match = Regex.Match(item, ValidationRegexPattern);
            if (match.Length < item.Length)
            {
                return new NameRuleError(NameRuleViolationInstance, _currentNamespaceName, _className, _currentMethodName, _parameterName);
            }
            return default(NameRuleError);
        }

       
    }
}
