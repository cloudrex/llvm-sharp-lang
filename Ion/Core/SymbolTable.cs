using System.Collections.Generic;
using Ion.CodeGeneration;
using LLVMSharp;

namespace Ion.Core
{
    /// <summary>
    /// Keeps track of emitted entities.
    /// </summary>
    public static class SymbolTable
    {
        public static readonly Dictionary<string, LLVMValueRef> functions = new Dictionary<string, LLVMValueRef>();

        /// <summary>
        /// Contains locally-scoped emitted values.
        /// All values are reset once the scope changes.
        /// </summary>
        public static readonly Dictionary<string, LLVMValueRef> localScope = new Dictionary<string, LLVMValueRef>();

        /// <summary>
        /// The active block being parsed.
        /// </summary>
        public static Block activeBlock;

        public static Dictionary<string, LLVMValueRef> strings = new Dictionary<string, LLVMValueRef>();

        /// <summary>
        /// Reset all temporary stored values.
        /// </summary>
        public static void Reset()
        {
            localScope.Clear();
            activeBlock = null;
        }

        public static void HardReset()
        {
            SymbolTable.Reset();
            SymbolTable.anonymousCounter = 0;
            SymbolTable.functions.Clear();
            SymbolTable.strings.Clear();
        }

        /// <summary>
        /// Incremental anonymous name counter.
        /// </summary>
        public static int anonymousCounter = 0;
    }
}
