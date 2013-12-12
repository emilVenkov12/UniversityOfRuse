using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Tests
{
    [TestClass]
    public class SyntacticAnalyzerTests
    {
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestMethodValidateCodeMissingEnd()
        {
            LexicalAnalyzer analyzator = new LexicalAnalyzer("class emo; begin A=A+C*D; end",
               new char[] { '=', ';', '.', ',', '*', '+', '|', '&', '<', '>', ')', '(' }, new List<string>() { "begin", "end", "int", "if" });
            SyntacticAnalyzer syntAnal = new SyntacticAnalyzer(analyzator);
            SymbolTable symbolTable = new SymbolTable(new ProgramSymbol("variable", SymbolClass.Identifier));
            syntAnal.ValidateCode(ref symbolTable);
            
        }
    }
}
