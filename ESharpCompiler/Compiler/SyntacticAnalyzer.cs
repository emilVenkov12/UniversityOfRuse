using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class Tetrade
    {
        public string OperationType;
        public ProgramSymbol FirstOperand;
        public ProgramSymbol SecondOperand;
        public ProgramSymbol Result;
        public int JumpDown;//-1
        public int JumpUp;//-1 no jump
    }
    public class SyntacticAnalyzer
    {
        #region Fields

        private LexicalAnalyzer lexAnalyzer;
        private ProgramSymbol currentProgramSymbol;
        private List<Tetrade> tetrades;

        private int sysVarsCount = 0;
        private int tetradesIndex = 0;

        #endregion

        #region Properties

        public LexicalAnalyzer LexAnalyzer
        {
            get
            {
                return this.lexAnalyzer;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("lexAnalyzer");
                }
                this.lexAnalyzer = value;
            }
        }

        public List<Tetrade> Tetrades
        {
            get
            {
                return new List<Tetrade>(this.tetrades);
            }
        }
        #endregion

        #region Constructors

        public SyntacticAnalyzer(LexicalAnalyzer lexAnalyzer)
        {
            this.LexAnalyzer = lexAnalyzer;
            this.tetrades = new List<Tetrade>();
        }

        #endregion

        #region Methods

        private ProgramSymbol CreateSystemVariable() 
        {
            this.sysVarsCount++;
            ProgramSymbol result = new ProgramSymbol("sysVar" + this.sysVarsCount, SymbolClass.Reserved);
            return result;
        }
        public void ValidateCode(ref SymbolTable symbolTable)
        {
            this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);

            if (this.currentProgramSymbol.Symbol != "class")
            {
                throw new FormatException("\"class\" keyword expected.");
            }

            this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
            if (this.currentProgramSymbol.SymbolClass != SymbolClass.Identifier)
            {
                throw new FormatException("Identifier expected.");
            }

            this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
            if (this.currentProgramSymbol.Symbol != ";")
            {
                throw new FormatException("; expected.");
            }

            this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);

            if (this.currentProgramSymbol.Symbol != "begin")
            {
                throw new FormatException("begin expected.");
            }

            ValidateCodeBlock(ref symbolTable);

            if (this.currentProgramSymbol.Symbol != "end")
            {
                throw new FormatException("end expected.");
            }

            this.tetrades.Add(new Tetrade() { OperationType = "STOP" });

        }

        private void ValidateCodeBlock(ref SymbolTable symbolTable)
        {
            this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);

            while (this.currentProgramSymbol != null && this.currentProgramSymbol.Symbol != "end")
            {
                if (this.currentProgramSymbol.Symbol == "begin")
                {
                    ValidateCodeBlock(ref symbolTable);

                    if (this.currentProgramSymbol == null || this.currentProgramSymbol.Symbol != "end")
                    {
                        throw new FormatException("end expected.");
                    }
                }
                else if (this.currentProgramSymbol.Symbol == "end")
                {
                    return;
                }
                else if (this.currentProgramSymbol.Symbol == "if" ||
                    this.currentProgramSymbol.Symbol == "while")
                {
                    string currentSymbol = this.currentProgramSymbol.Symbol;
                    this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
                    

                    ValideteCondition(ref symbolTable);

                    this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
                    int jumpIndex = this.tetrades.Count - 1;

                    if (this.currentProgramSymbol.Symbol != "begin")
                    {
                        throw new FormatException("begin expected.");
                    }

                    ValidateCodeBlock(ref symbolTable);

                    if (this.currentProgramSymbol.Symbol != "end")
                    {
                        throw new FormatException("end expected.");
                    }
                    if (currentSymbol == "while")
                    {
                        this.tetrades.Add(new Tetrade() 
                        {
                            OperationType = "JMP",
                            JumpUp = jumpIndex,
                            JumpDown = -1
                        });
                        
                    }
                    this.tetrades[jumpIndex].JumpDown = this.tetrades.Count;
                    
                }
                //else if (this.currentProgramSymbol.Symbol == "int" ||
                //    this.currentProgramSymbol.Symbol == "float" ||
                //    this.currentProgramSymbol.Symbol == "char")
                //{
                //    this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
                //    if (this.currentProgramSymbol.SymbolClass != SymbolClass.Identifier)
                //    {
                //        throw new FormatException("Identifier name expected.");
                //    }
                //    this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
                //    if (this.currentProgramSymbol.Symbol == "=")
                //    {
                //        string argument = string.Empty;
                //        ValidateExpresion(ref symbolTable, ref argument);
                //    }

                //    if (this.currentProgramSymbol.Symbol != ";")
                //    {
                //        throw new FormatException("; expected.");
                //    }
                //}
                else if (this.currentProgramSymbol.SymbolClass == SymbolClass.Identifier)
                {
                    ProgramSymbol variable = new ProgramSymbol(this.currentProgramSymbol.Symbol, SymbolClass.Identifier);
                    this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
                    if (this.currentProgramSymbol.Symbol != "=")
                    {
                        throw new FormatException("\"=\" expected.");
                    }
                    this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);

                    ProgramSymbol value = this.CreateSystemVariable();

                    ValidateExpresion(ref symbolTable, ref value);
                    
                    this.tetrades.Add(new Tetrade() { FirstOperand = variable, 
                        OperationType = "=", 
                        SecondOperand = null, JumpDown = -1, JumpUp = -1, Result = value });

                    if (this.currentProgramSymbol.Symbol != ";")
                    {
                        throw new FormatException("; expected.");
                    }
                }

                this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
            }
        }

        private void ValidateExpresion(ref SymbolTable symbolTable, ref ProgramSymbol operand)
        {
            this.ValidateTerm(ref symbolTable, ref operand);
            while (this.currentProgramSymbol.Symbol == "+" || this.currentProgramSymbol.Symbol == "-")
            {
                string operationType = this.currentProgramSymbol.Symbol;

                this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);

                ProgramSymbol secondOperand = this.CreateSystemVariable();

                this.ValidateTerm(ref symbolTable, ref secondOperand);

                ProgramSymbol result = this.CreateSystemVariable();
                this.tetrades.Add(new Tetrade() { 
                    FirstOperand = operand, 
                    SecondOperand = secondOperand, 
                    OperationType = operationType, 
                    Result = result,
                    JumpDown = -1,
                    JumpUp = -1
                });
                operand = result;
            }
        }

        private void ValidateTerm(ref SymbolTable symbolTable, ref ProgramSymbol operand)
        {
            this.ValidateFactor(ref symbolTable, ref operand);
            while (this.currentProgramSymbol.Symbol == "*" || this.currentProgramSymbol.Symbol == "/")
            {
                string operationType = this.currentProgramSymbol.Symbol;
                this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);

                ProgramSymbol secondOperand = CreateSystemVariable();
                this.ValidateFactor(ref symbolTable, ref secondOperand);
                ProgramSymbol result = this.CreateSystemVariable();
                this.tetrades.Add(new Tetrade()
                {
                    FirstOperand = operand,
                    SecondOperand = secondOperand,
                    OperationType = operationType,
                    Result = result
                });
                operand = result;
            }
        }

        private void ValidateFactor(ref SymbolTable symbolTable, ref ProgramSymbol operand)
        {
            if (this.currentProgramSymbol.Symbol == "(")
            {
                this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
                this.ValidateExpresion(ref symbolTable, ref operand);
                if (this.currentProgramSymbol.Symbol == ")")
                {
                    this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
                }
                else
                {
                    throw new FormatException(") expected");
                }
            }
            else if (this.currentProgramSymbol.SymbolClass == SymbolClass.Identifier ||
                this.currentProgramSymbol.SymbolClass == SymbolClass.Number)
            {
                operand = this.currentProgramSymbol;
                this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
            }
            else
            {
                throw new FormatException("Operand expected");
            }
        }

        private void ValideteCondition(ref SymbolTable symbolTable)
        {
            //this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
            if (this.currentProgramSymbol.Symbol != "(")
            {
                throw new FormatException("( expected");
            }
            ProgramSymbol operand = this.CreateSystemVariable();
            this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
            ValidateSubCondition(ref symbolTable, ref operand);
            this.tetrades.Add(new Tetrade() 
            {
                OperationType = "BRZ",
                FirstOperand = operand,
            });

            if (this.currentProgramSymbol.Symbol != ")")
            {
                throw new FormatException(") expected");
            }

            //ValidateCodeBlock(ref symbolTable);
        }

        private void ValidateSubCondition(ref SymbolTable symbolTable, ref ProgramSymbol operand)
        {
            ValidateExpresion(ref symbolTable, ref operand);

            if (this.currentProgramSymbol.Symbol == ">" ||
                this.currentProgramSymbol.Symbol == "<" ||
                this.currentProgramSymbol.Symbol == "==")
            {
                string operationType = this.currentProgramSymbol.Symbol;

                this.currentProgramSymbol = this.lexAnalyzer.GetNextProgramSymbol(ref symbolTable);
                ProgramSymbol secondOperand = this.CreateSystemVariable();

                this.ValidateExpresion(ref symbolTable, ref secondOperand);

                ProgramSymbol result = this.CreateSystemVariable();
                this.tetrades.Add(new Tetrade
                {
                    OperationType = operationType,
                    FirstOperand = operand,
                    SecondOperand = secondOperand,
                    Result = result,
                    JumpDown = -1,
                    JumpUp = -1
                });

                operand = result;
            }
            else
            {
                throw new FormatException("expected >, <, =");
            }
        }

        #endregion
    }
}
