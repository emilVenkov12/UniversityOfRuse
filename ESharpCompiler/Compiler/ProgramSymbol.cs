using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class ProgramSymbol
    {
        #region Fields

        private string symbol;
        private SymbolClass symbolClass;
        private string value;

        #endregion

        #region Properties

        public string Symbol 
        {
            get { return this.symbol; }
            private set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("The symbol cannot be null or empty");
                }
                this.symbol = value;
            }
        }
        public SymbolClass SymbolClass 
        {
            get { return this.symbolClass; }
            private set 
            {
                this.symbolClass = value;
            }
        }

        public string Value
        {
            get { return this.value; }
            private set
            {
                if (value == String.Empty)
                {
                    throw new ArgumentException("The value of the symbol cannot be empty");
                }
                this.value = value;
            }
        }

        #endregion

        #region Constructors
        public ProgramSymbol(string symbol, SymbolClass symbolClass)
            : this(symbol, symbolClass, null)
        { }

        public ProgramSymbol(string symbol, SymbolClass symbolClass, string value)
        {
            this.Symbol = symbol;
            this.SymbolClass = symbolClass;
            this.Value = value;
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            ProgramSymbol other = obj as ProgramSymbol;

            if (other == null)
            {
                return false;
            }

            return this.symbol == other.symbol && this.symbolClass == other.symbolClass;
        }

        #endregion

        
    }
}

