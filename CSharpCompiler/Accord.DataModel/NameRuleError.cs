using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using Prism.Mvvm;

namespace Accord.DataModel
{

    public class NameRuleError : BindableBase
    {
        public readonly NameRuleViolations Violation;
        //public readonly string Namespace;
        //public readonly string ClassName;
        //public readonly string Method;
        //public readonly string Parameter;

        private string nameSpaceName;

        public string NameSpace
        {
            get { return nameSpaceName; }
            set
            {
                nameSpaceName = value;
                RaisePropertyChanged(nameof(NameSpace));
            }
        }



        public string NameRuleViolationString
        {
            get { return Violation.ToString(); }
        }

        private string className;

        public string ClassName
        {
            get { return className; }
            set
            {
                className = value;
                RaisePropertyChanged(nameof(ClassName));
            }
        }

        private string methodName;

        public string Method
        {
            get { return methodName; }
            set { methodName = value; RaisePropertyChanged(nameof(Method)); }
        }

        private string _parameter;

        public string Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                RaisePropertyChanged(nameof(Parameter));
            }
        }
                

        public string Suggestion
        {
            get { return GetSuggestion(); }            
        }


        public NameRuleError(NameRuleViolations violation, string nameSpace, string className, string method, string parameter)
        {
            Violation = violation;
            NameSpace = nameSpace;
            ClassName = className;
            Method = method;
            Parameter = parameter;
        }

        public virtual string GetErrorMessage()
        {
            var result = string.Format("{0} at {1} {2} {3}", Violation.ToString(), NameSpace, ClassName, Method, Parameter);
            return result;
        }

        public virtual string GetSuggestion()
        {
            var desc = EnumDescriptionFetcher.GetDescription<NameRuleViolations>(Violation);
            return desc;
        }
    }
}
