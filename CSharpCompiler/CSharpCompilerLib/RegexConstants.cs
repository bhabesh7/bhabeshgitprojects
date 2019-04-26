using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCompilerLib
{
    public class RegexConstants
    {
        public const string PascalCaseClassMethodRegex = "[A-Z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";
        public const string CamelCaseParameterRegex = "[a-z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";
        public const string PascalCaseNamespaceRegex = "[A-Z]([A-Z0-9]*[a-z][a-z0-9]*[A-Z]|[a-z0-9]*[A-Z][A-Z0-9]*[a-z])[A-Za-z0-9]*";
        public const string PrivatePropertyRegex = "^_[a-z]+[A-Z0-9]+[a-z0-9]*";
        public const string PublicPropertyRegex = "^[A-Z]+[a-z0-9]+[A-Z0-9]*[a-z0-9]+";

    }
}
