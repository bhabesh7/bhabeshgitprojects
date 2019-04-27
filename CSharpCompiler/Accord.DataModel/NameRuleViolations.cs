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
        [Description("Default Violation State")]
        Default,
        [Description("MethodName must follow Pascal case. Eg. MySampleMethod")]
        MethodNameRuleViolation,
        [Description("Method ParameterName must follow camel case. Eg. mySampleParam1")]
        ParameterNameRuleViolation,
        [Description("ClassName must follow Pascal case. Eg. MySampleClassName1")]
        ClassNameRuleViolation,
        [Description("Namespace must follow pattern for Eg. MySampleNameSpace or MySampleNameSpace.Package*")]
        NamespaceRuleViolation,
        [Description("Public Field name must follow Pascal case. Eg. MySampleNameSpace or MySampleNameSpace.Package")]
        PublicFieldNameRuleViolation,
        [Description("Protected Field name must follow Pascal case. Eg. MySampleNameSpace or MySampleNameSpace.Package")]
        ProtectedFieldNameRuleViolation,
        [Description("Private Field name must follow camel case starting with _ for Eg. _myFieldName")]
        PrivateFieldNameRuleViolation,
        [Description("Property name must follow Pascal case. Eg. MySampleNameSpace or MySampleNameSpace.Package")]
        PropertyNameRuleViolation,
        [Description("Public Property name must follow Pascal case. Eg. MySampleNameSpace or MySampleNameSpace.Package")]
        PublicPropertyNameRuleViolation,
        [Description("Protected Property name must follow Pascal case. Eg. MySampleNameSpace or MySampleNameSpace.Package")]
        ProtectedPropertyNameRuleViolation,
        [Description("Private Property name must follow camel case starting with _ for Eg. _myFieldName")]
        PrivatePropertyNameRuleViolation,
        [Description("Method Body must be less than 50 lines")]
        LargeMethodBodyRuleViolation,
        [Description("Name must be less than 50 characters")]
        NameLengthExceededRuleViolation
    }
}
