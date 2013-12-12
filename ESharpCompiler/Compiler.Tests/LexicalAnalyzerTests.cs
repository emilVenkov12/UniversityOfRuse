using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Compiler.Tests
{
    [TestClass]
    public class LexicalAnalyzerTests
    {
        [TestMethod]
        public void TestMethodGetNextProgramSymbolIdentifier()
        {
            LexicalAnalyzer analyzator = new LexicalAnalyzer("beg&^$#@!in\n\tint p  = 7; \n end.",
                new char[] { '=', ';', '.' }, new List<string>() { "begin", "end", "int" });

            var expected = new ProgramSymbol("beg&^$#@!in", SymbolClass.Identifier);
            SymbolTable symbolTable = new SymbolTable(new ProgramSymbol("variable", SymbolClass.Identifier));
            var actual = analyzator.GetNextProgramSymbol(ref symbolTable);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodGetNextProgramSymbolReservedSymbols()
        {
            LexicalAnalyzer analyzator = new LexicalAnalyzer("begin\n\tint p c h s ss",
                new char[] { '=', ';', '.' }, new List<string>() { "begin", "end", "int" });

            var expected = new ProgramSymbol("begin", SymbolClass.Reserved);
            SymbolTable symbolTable = new SymbolTable(new ProgramSymbol("variable", SymbolClass.Identifier));
            var actual = analyzator.GetNextProgramSymbol(ref symbolTable);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodGetNextProgramSymbolCEmptyFileContent()
        {
            LexicalAnalyzer analyzator = new LexicalAnalyzer("", new char[] { '=', ';', '.' }, 
                new List<string>() { "begin", "end", "int" });

            object expected = null;
            SymbolTable symbolTable = new SymbolTable(new ProgramSymbol("variable", SymbolClass.Identifier));
            var actual = analyzator.GetNextProgramSymbol(ref symbolTable);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodGetNextProgramSymbolCheckReadedList()
        {
            LexicalAnalyzer analyzator = new LexicalAnalyzer("begin=\n\tint p h  ,",
                new char[] { '=', ';', '.', ',', ')', '(', '+', '-', '/', '*' }, 
                new List<string>() { "begin", "end", "int" });
           
            var expected = new List<ProgramSymbol>();
            expected.Add(new ProgramSymbol("begin", SymbolClass.Reserved));
            expected.Add(new ProgramSymbol("=", SymbolClass.Operator));
            expected.Add(new ProgramSymbol("int", SymbolClass.Reserved));
            expected.Add(new ProgramSymbol("p", SymbolClass.Identifier));
            expected.Add(new ProgramSymbol("h", SymbolClass.Identifier));
            expected.Add(new ProgramSymbol(",", SymbolClass.Operator));
            SymbolTable symbolTable = new SymbolTable(new ProgramSymbol("variable", SymbolClass.Identifier));

            while (analyzator.GetNextProgramSymbol(ref symbolTable) != null) ;
            
            var actual = analyzator.ReadedSymbols;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestMethodGetNextProgramSymbolUnknowSymbol()
        {
            LexicalAnalyzer analyzator = new LexicalAnalyzer("begin\n\tint p h  ,",
                new char[] { '=', ';', '.' }, new List<string>() { "begin", "end", "int" });

            var expected = new List<ProgramSymbol>();
            expected.Add(new ProgramSymbol("begin", SymbolClass.Identifier));
            expected.Add(new ProgramSymbol("int", SymbolClass.Identifier));
            expected.Add(new ProgramSymbol("p", SymbolClass.Identifier));
            expected.Add(new ProgramSymbol("h", SymbolClass.Identifier));
            SymbolTable symbolTable = new SymbolTable(new ProgramSymbol("variable", SymbolClass.Identifier));

            while (analyzator.GetNextProgramSymbol(ref symbolTable) != null) ;

            var actual = analyzator.ReadedSymbols;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodGetNextProgramSymbolIntNumber()
        {
            LexicalAnalyzer analyzator = new LexicalAnalyzer("123",
                new char[] { '=', ';', '.' }, new List<string>() { "begin", "end", "int" });

            var expected = new ProgramSymbol("123", SymbolClass.Number);
            SymbolTable symbolTable = new SymbolTable(new ProgramSymbol("variable", SymbolClass.Identifier));

            var actual = analyzator.GetNextProgramSymbol(ref symbolTable);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodGetNextProgramSymbolFloatNumber()
        {            
            LexicalAnalyzer analyzator = new LexicalAnalyzer("123,345",
                new char[] { '=', ';', '.' }, new List<string>() { "begin", "end", "int" });

            var expected = new ProgramSymbol("123,345", SymbolClass.Number);
            SymbolTable symbolTable = new SymbolTable(new ProgramSymbol("variable", SymbolClass.Identifier));

            var actual = analyzator.GetNextProgramSymbol(ref symbolTable);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodGetNextProgramSymbolComplexTest()
        {
            LexicalAnalyzer analyzator = new LexicalAnalyzer("begin6871326_&^%$^*) 123 12,4=\n\tint p h  ,",
                new char[] { '=', ';', '.', ',', ')', '(', '+', '-', '/', '*' }, 
                new List<string>() { "begin", "end", "int" });

            var expected = new List<ProgramSymbol>();
            expected.Add(new ProgramSymbol("begin6871326_&^%$^", SymbolClass.Identifier));
            expected.Add(new ProgramSymbol("*", SymbolClass.Operator));
            expected.Add(new ProgramSymbol(")", SymbolClass.Operator));
            expected.Add(new ProgramSymbol("123", SymbolClass.Number));
            expected.Add(new ProgramSymbol("12,4", SymbolClass.Number));
            expected.Add(new ProgramSymbol("=", SymbolClass.Operator));
            expected.Add(new ProgramSymbol("int", SymbolClass.Reserved));
            expected.Add(new ProgramSymbol("p", SymbolClass.Identifier));
            expected.Add(new ProgramSymbol("h", SymbolClass.Identifier));
            expected.Add(new ProgramSymbol(",", SymbolClass.Operator));
            SymbolTable symbolTable = new SymbolTable(new ProgramSymbol("variable", SymbolClass.Identifier));

            while (analyzator.GetNextProgramSymbol(ref symbolTable) != null) ;

            var actual = analyzator.ReadedSymbols;
            CollectionAssert.AreEqual(expected, actual);
        }
       
    }
}
