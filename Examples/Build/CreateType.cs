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
DynamicAssembly dyn = new DynamicAssembly("MyAssembly", new Version(1, 0, 0, 0));

            // Store the runtime lib, which contains .NET fundamental types.
            ILMechanics.Entities.AssemblyReference rt = dyn.StoreAssemblyReference("System.Runtime", new Version(7, 0, 0, 0));
            rt.PublicKey = new byte[] { 0xb0, 0x3f, 0x5f, 0x7f, 0x11, 0xd5, 0x0a, 0x3a };

            // Store a reference of System.Object or simply object.
            ILTypeReference obj = dyn.StoreTypeReference("System", "Object", rt);

            // Store constructor method of System.Object.
            ILMemberReference objCtor = dyn.StoreMemberReference(".ctor", ILSignature.Void(), obj);

            // Define the type by providing the name and namespace.
            ILType type = dyn.DefineType("MyClass", "MyNamespace");

            // Define the type attributes.
            type.Attributes = TypeAttributes.Public | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit;

            // Set the base type to `object`.
            type.BaseType = obj.ReferenceSignature();

            // Define the method by indicating the type that defines it.
            ILMethod constructor = dyn.DefineMethod(type);

            // Assign the name and attributes for constructors.
            constructor.Name = ".ctor";
            constructor.Attributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.HideBySig;

            // Set the return type, which in constructors is always `void`.
            constructor.ReturnType = ILSignature.Void();

            // Set the body.
            constructor.SetBody(
                InstructionBuilder.IL(ILOpCode.Ldarg_0),
                InstructionBuilder.IL(ILOpCode.Call, objCtor.ReferenceSignature()),
                InstructionBuilder.IL(ILOpCode.Ret)
                );

            byte[] file = dyn.Emit();

            File.WriteAllBytes(@"C:\Users\nicog\source\repos\Draft\Draft\ILAssembly.dll", file);
        }
    }
}
