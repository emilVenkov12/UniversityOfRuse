using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Compiler.Tests
{
    [TestClass]
    public class SymbolTableTests
    {
        [TestMethod]
        public void TestMethodAddSymbol()
        {
            SymbolTable st = new SymbolTable(new ProgramSymbol("begin", SymbolClass.Identifier));
            st.AddSymbol(new ProgramSymbol("end", SymbolClass.Identifier));

            var expected = new ProgramSymbol("begin", SymbolClass.Identifier);
            var actual = st.GetProgramSymbol("begin");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodAdd5000SymbolsAndSearch()
        {
            SymbolTable st = new SymbolTable(new ProgramSymbol("begin", SymbolClass.Identifier));
            for (int i = 0; i < 5000; i++)
            {
                st.AddSymbol(new ProgramSymbol("end" + i, SymbolClass.Identifier)); 
            }
            var expected = new ProgramSymbol("end4990", SymbolClass.Identifier);
            var actual = st.GetProgramSymbol("end4990");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethodGetProgramSymbolMissingSymbol()
        {
            SymbolTable st = new SymbolTable(new ProgramSymbol("begin", SymbolClass.Identifier));
            
            ProgramSymbol expected = null;
            var actual = st.GetProgramSymbol("end4990");
            Assert.AreEqual(expected, actual);
        }
    }
}
