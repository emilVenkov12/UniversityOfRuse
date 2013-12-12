using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public enum SymbolClass
    {
        Reserved = 1,
        Identifier = 2,
        Number = 3,
        Operator = 4,
    }
}
