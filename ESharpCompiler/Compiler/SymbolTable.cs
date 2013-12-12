using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace Compiler
{
    public class SymbolTable
    {
        #region Fields

        //private Set<ProgramSymbol> programSymbols;
        private HashSet<ProgramSymbol> programSymbols;

        #endregion

        #region  Constructors

        public SymbolTable(params ProgramSymbol[] parameters)
        {
            this.programSymbols = new HashSet<ProgramSymbol>();
            
            //this.programSymbols = new Set<ProgramSymbol>();

            foreach (var parameter in parameters)
            {
                this.programSymbols.Add(parameter);
            }
        }

        #endregion

        #region Methods

        public void AddSymbol(ProgramSymbol symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException("symbol");
            }
            if (this.programSymbols.Contains(symbol))
            {
                throw new ArgumentException("The program symbol which you try to add already exist!");
            }
            this.programSymbols.Add(symbol);
        }

        public ProgramSymbol GetProgramSymbol(string symbol)
        {
            ProgramSymbol result = Algorithms.FindWhere(this.programSymbols, x => x.Symbol == symbol).FirstOrDefault();

            return result as ProgramSymbol;
        }

        #endregion
    }
}
