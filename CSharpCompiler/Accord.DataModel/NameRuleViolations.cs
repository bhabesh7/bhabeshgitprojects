using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accord.DataModel
{
    public enum NameRuleViolations
    {
        [Description("MethodName must follow Pascal case. Eg. MySampleMethod")]
        MethodNameRuleViolation,
        [Description("Method ParameterName must follow camel case. Eg. mySampleParam1")]
        ParameterNameRuleViolation,
        [Description("ClassName must follow Pascal case. Eg. MySampleClassName1")]
        ClassNameRuleViolation,
        [Description("Namespace must follow Pascal case. Eg. MySampleNameSpace or MySampleNameSpace.Package")]
        NamespaceRuleViolation,
        [Description("Field name must follow Pascal case. Eg. MySampleNameSpace or MySampleNameSpace.Package")]
        FieldNameRuleViolation,
        [Description("Property name must follow Pascal case. Eg. MySampleNameSpace or MySampleNameSpace.Package")]
        PropertyNameRuleViolation
    }
}
