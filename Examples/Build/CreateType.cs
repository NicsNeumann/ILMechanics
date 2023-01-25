/*
*
* The following example shows how to create a new assembly and define a type in it.
*
*/

using ILMechanics;
using ILMechanics.Csharp;
using ILMechanics.Entities;

namespace MyScope
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            // Create an assembly builder by passing a name, version and runtime lib descriptor.
            var runtimeLib = new AssemblyName("System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=null");
            var descriptor = AssemblyDescriptor.Create("ILAssembly", new Version(1, 0, 0, 0), runtimeLib);
            var builder = AssemblyBuilder.Create(descriptor);

            // Get the underlying assembly.
            var dyn = builder.Assembly;

            // Set the namespace where the type will be defined.
            builder.CurrentNamespace = "ILExample";

            // Create a new public class.
            var type = builder.ClassBuilder("ILClass", AccessModifiers.Public);

            // Add a method to the class and mark it as the default constructor.
            var mth = type.AddMethod(AssemblyBuilder.Ctor, AccessModifiers.Public, ILSignature.Void());
            mth.DefaultConstructor();

            // Resolve all entities and emit the assembly to the specified path.
            builder.Resolve();
            dyn.Emit(@"X:/.../path/to/file");
        }
    }
}
