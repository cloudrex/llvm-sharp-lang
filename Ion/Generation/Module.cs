using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ion.Generation.Helpers;
using LLVMSharp;

namespace Ion.Generation
{
    public class Module : IDisposable, ICloneable
    {
        public LLVMModuleRef Target { get; }

        public string Identifier { get; set; }

        public string FileName { get; }

        public List<string> Imports { get; }

        public Module(string fileName, LLVMModuleRef target)
        {
            this.Target = target;
            this.FileName = fileName;

            // Create imports.
            this.Imports = new List<string>();
        }

        public Module(string fileName, string identifier) : this(fileName, LLVM.ModuleCreateWithNameInContext(identifier, LLVM.GetGlobalContext()))
        {
            // TODO: Restrict to identifier pattern (use Util.ValidateIdentifier()).
            this.Identifier = identifier;
        }

        public Module(string fileName) : this(fileName, SpecialName.Entry)
        {
            //
        }

        public object Clone()
        {
            return new Module(this.FileName, LLVM.CloneModule(this.Target));
        }

        public void Dispose()
        {
            LLVM.DisposeModule(this.Target);
        }

        // TODO: Throws "AccessViolationException" (uncatchable) exception if function does not exist.
        // TODO: ... determine a way to see if the function exists first. LLVM.IsNull() and LLVMValueRef.IsNull()
        // TODO: ... all trigger the exception too.
        public LLVMValueRef GetFunction(string name)
        {
            // Retrieve the function.
            LLVMValueRef function = LLVM.GetNamedFunction(this.Target, name);

            // Return the retrieved function.
            return function;
        }

        public LLVMContextRef GetContext()
        {
            return LLVM.GetModuleContext(this.Target);
        }

        /// <summary>
        /// Dump the contents of the corresponding
        /// IR code with this module to the console
        /// output.
        /// </summary>
        public void Dump()
        {
            LLVM.DumpModule(this.Target);
        }

        /// <summary>
        /// Obtain the corresponding IR code string from this
        /// module.
        /// </summary>
        public string Emit()
        {
            // Apply the module's identifier.
            LLVM.SetModuleIdentifier(this.Target, this.Identifier, this.Identifier.Length);

            // Print IR code to a buffer.
            IntPtr output = LLVM.PrintModuleToString(this.Target);

            // Convert buffer to a string.
            string outputString = Marshal.PtrToStringAnsi(output);

            // Dispose message.
            LLVM.DisposeMessage(output);

            // Trim whitespace.
            outputString = outputString.Trim();

            // Return resulting output string.
            return outputString;
        }
    }
}
