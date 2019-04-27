using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCompilerLib
{
    public class RegexConstants
    {
        public const string PascalCaseClassRegex = "(^[A-Z]([A-Za-z0-9*])+)*"; //"^[A-Z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";
        public const string PascalCaseMethodRegex = "(^[A-Z]([A-Za-z0-9_*])+)*";//"^[A-Z]([A-Z0-9]*[a-z][a-z0-9]*_|[a-z0-9]*[A-Z][A-Z0-9]*_)[A-Za-z0-9]*";

        //public const string CamelCaseParameterRegex = "[a-z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";
        public const string CamelCaseParameterRegex = "^[a-z]+[A-Z0-9]+[a-z0-9]*";
        public const string PascalCaseNamespaceRegex = "(^[A-Z]([A-Za-z0-9.*])+)*";
        public const string PrivatePropertyRegex = "(^[a-z]([A-Za-z0-9_*])+)*";//"^_[a-z]+[A-Z0-9]+[a-z0-9]*";
        public const string PublicPropertyRegex = "(^[A-Z]([A-Za-z0-9_*])+)*"; //"^[A-Z]+[a-z0-9]+[A-Z0-9]*[a-z0-9]+";

    }
}
