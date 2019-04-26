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
        private string _className;
        private string _currentNamespaceName;
        private string _currentMethodName;
        private string _parameterName;
        private string _propertyOrFieldName;
        
        protected string ValidationRegexPattern { get; set; }
        protected NameRuleViolations NameRuleViolationInstance { get; set; }

        public BaseValidationAttribute()
        {            
            NameRuleViolationInstance = NameRuleViolations.Default;
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

        protected NameRuleError ValidateString(string item)
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
