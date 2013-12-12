using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Compiler
{
    public class LexicalAnalyzer
    {
        #region Fields

        private string code;
        private int lastSymbolAnalyzed;
        private List<ProgramSymbol> readedSymbols;
        private char[] operators;
        private List<string> reservedSymbols;

        #endregion

        #region Properties

        public string Code
        {
            get
            {
                return this.code;
            }
            private set 
            {
                if (value == null)
                {
                    throw new ArgumentException("fileContetnt cannot be null!");
                }
                this.code = value;
            }
        }

        public List<ProgramSymbol> ReadedSymbols {
            get 
            { 
                return new List<ProgramSymbol>(this.readedSymbols); 
            }
            private set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException("readedSymbols");
                }
                this.readedSymbols = value;
            }
        }

        public char[] Operators 
        {
            get 
            {
                return this.operators;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("operators");
                }
                this.operators = value;
            }
        }

        public List<string> ReservedSymbols
        {
            get
            {
                return new List<string>(this.reservedSymbols);
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("reservedSymbols");
                }
                this.reservedSymbols = value;
            }
        }

        #endregion

        #region Constructors

        public LexicalAnalyzer(string code, char[] operators, List<string> reservedSymbols)
        {
            this.Code = code;
            this.lastSymbolAnalyzed = 0;
            this.readedSymbols = new List<ProgramSymbol>();
            this.Operators = operators;
            this.ReservedSymbols = reservedSymbols;
        }

        #endregion

        #region Methods
        //TODO: optimization
        public ProgramSymbol GetNextProgramSymbol(ref SymbolTable symbolTable)
        {
            if (this.lastSymbolAnalyzed >= this.code.Length)
            {
                return null;
            }

            while (this.lastSymbolAnalyzed < this.code.Length &&
                char.IsWhiteSpace(this.code[this.lastSymbolAnalyzed]))
            {
                this.lastSymbolAnalyzed++;
            }

            if (this.lastSymbolAnalyzed >= this.code.Length)
            {
                return null;
            }

            if (char.IsLetter(this.code[this.lastSymbolAnalyzed]))
            {
                ProgramSymbol result = ProcessIdentifier(operators);
                this.readedSymbols.Add(result);
                if (symbolTable.GetProgramSymbol(result.Symbol) == null)
                {
                    symbolTable.AddSymbol(result);
                }
                return result;
            }
            else if (char.IsNumber(this.code[this.lastSymbolAnalyzed]))
            {
                ProgramSymbol result = ProcessNumber();
                this.readedSymbols.Add(result);
                if (symbolTable.GetProgramSymbol(result.Symbol) == null)
                {
                    symbolTable.AddSymbol(result);
                }
                return result;
            }
            else if (operators.Contains(this.code[this.lastSymbolAnalyzed]))
            {
                if (this.code[this.lastSymbolAnalyzed] == '=')
                {
                    if (this.lastSymbolAnalyzed + 1 < this.code.Length && 
                        this.code[this.lastSymbolAnalyzed + 1] == '=')
                    {
                        this.lastSymbolAnalyzed += 2;
                        ProgramSymbol result = new ProgramSymbol("==", SymbolClass.Operator);
                        this.readedSymbols.Add(result);
                        if (symbolTable.GetProgramSymbol(result.Symbol) == null)
                        {
                            symbolTable.AddSymbol(result);
                        }
                        return result;
                    }
                    else
                    {
                        ProgramSymbol result = new ProgramSymbol(this.code[this.lastSymbolAnalyzed] + "", SymbolClass.Operator);
                        this.readedSymbols.Add(result);
                        this.lastSymbolAnalyzed++;
                        if (symbolTable.GetProgramSymbol(result.Symbol) == null)
                        {
                            symbolTable.AddSymbol(result);
                        }
                        return result;
                    }
                }
                else
                {
                    ProgramSymbol result = new ProgramSymbol(this.code[this.lastSymbolAnalyzed] + "", SymbolClass.Operator);
                    this.readedSymbols.Add(result);
                    this.lastSymbolAnalyzed++;
                    if (symbolTable.GetProgramSymbol(result.Symbol) == null)
                    {
                        symbolTable.AddSymbol(result);
                    }
                    return result;
                }
            }
            else
            {
                throw new FormatException("Unknown symbol! \" " + this.code[this.lastSymbolAnalyzed] + "\"");
            }
        }

        private ProgramSymbol ProcessNumber()
        {
            int index = this.lastSymbolAnalyzed;
            int commaCount = 0;
            StringBuilder symbol = new StringBuilder();
            symbol.Append(this.code[index]);
            index++;
            while (index < this.code.Length &&
                (char.IsNumber(this.code[index]) || this.code[index] == ',') && commaCount < 1)
            {
                if (this.code[index] == ',')
                {
                    commaCount++;
                }
                symbol.Append(this.code[index]);
                index++;
            }

            this.lastSymbolAnalyzed = index;
            return new ProgramSymbol(symbol.ToString(), SymbolClass.Number, symbol.ToString());
        }

        private ProgramSymbol ProcessIdentifier(char[] operators)
        {
            int index = this.lastSymbolAnalyzed;
            StringBuilder symbol = new StringBuilder();
            symbol.Append(this.code[index]);
            index++;
            while (index < this.code.Length &&
                !char.IsWhiteSpace(this.code[index]) &&
                !operators.Contains(this.code[index]))
            {
                symbol.Append(this.code[index]);
                index++;
            }

            this.lastSymbolAnalyzed = index;
            SymbolClass currentSymbolClass = SymbolClass.Identifier;
            if (this.reservedSymbols.Contains(symbol.ToString()))
            {
                currentSymbolClass = SymbolClass.Reserved;
            }
            ProgramSymbol result = new ProgramSymbol(symbol.ToString(), currentSymbolClass);
            
            return result;
        }

        static void Main(string[] args)
        {
            //main algorithum
            string text = @"begin
    int p = 5;
    if ( p == 5 )
    begin
        p = p + 5;
    end;
end.";
            List<string> tokens = new List<string>();
            HashSet<string> reservedSymbols = new HashSet<string>() { "begin", "end", "int", "bool", "float", "char", "if", "else", "'", ".", ";", "+", "=", "==" };
            StringBuilder token = new StringBuilder();


            for (int index = 0; index < text.Length; index++)
            {
                char currentChar = text[index];

                if (reservedSymbols.Contains(currentChar.ToString()))
                {
                    if (token.Length > 0)
                    {
                        tokens.Add(token.ToString());
                    }
                    tokens.Add(currentChar.ToString().Trim());
                    token.Clear();
                }
                else if (currentChar == ' ' && token.Length > 0)
                {
                    tokens.Add(token.ToString().Trim());
                    token.Clear();
                }
                else if (!char.IsWhiteSpace(currentChar))
                {
                    token.Append(currentChar.ToString());
                }
            }
        }

        #endregion
    }
}
