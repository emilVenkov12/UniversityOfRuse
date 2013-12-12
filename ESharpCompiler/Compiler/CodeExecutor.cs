namespace Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    class CodeExecutor
    {
        private List<Tetrade> tetrades = new List<Tetrade>();


        public CodeExecutor(List<ProgramSymbol> tetrades)
        {
            if (tetrades == null)
            {
                throw new ArgumentNullException();
            }
            this.tetrades = tetrades;
        }

        public void SaveToAsmFile(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("The file path should be non-empty and not equal to null.");
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                for (int i = 0; i < this.tetrades.Count; i++)
                {
                    //chek for label here !!
                    switch (this.tetrades[i].OperationType)
                    {
                        case "+": 
                            {
                                sw.WriteLine("LDA " + this.tetrades[i].FirstOperand.Symbol);
                                sw.WriteLine("ADD " + this.tetrades[i].SecondOperand.Symbol);
                                sw.WriteLine("STA " + this.tetrades[i].Result.Symbol);
                            }
                            break;
                        case "-": 
                            {
                                sw.WriteLine("LDA " + this.tetrades[i].FirstOperand.Symbol);
                                sw.WriteLine("SUB " + this.tetrades[i].SecondOperand.Symbol);
                                sw.WriteLine("STA " + this.tetrades[i].Result.Symbol);
                            }
                        break;
                        case "*": 
                            {
                                sw.WriteLine("LDA " + this.tetrades[i].FirstOperand.Symbol);
                                sw.WriteLine("MUL " + this.tetrades[i].SecondOperand.Symbol);
                                sw.WriteLine("STA " + this.tetrades[i].Result.Symbol);
                            }
                            break;
                        case "/": 
                            {

                            }
                            break;
                        default:
                            break;
                    }
                }   
            }
        }


    }
}
