using CodeObfuscator.MonoBehaviour;
using dnlib.DotNet;

namespace CodeObfuscator.Transform
{
    internal sealed class RenameTransformer : Transformer
    {
        public RenameTransformer(ModuleDefMD module) : base(module,"Renamer", TransformType.Rename)
        {
            this.Module = module;
        }

        //Preforms Name transform on module (.exe or .dll file) for valid methods, fields and parameters
        public override void Transform(ModuleDefMD module)
        {
            int typesIndex = 1;
            int methodsIndex = 1;
            int paramIndex = 1;
            int fieldIndex = 1;
            //int variablesIndex = 1;
            foreach(var type in module.Types)
            {
                //if (type.Name == "<Module>" || type.Namespace == "<Module>")
                //    continue;
                Console.WriteLine("*********** Statring to preform changes on Type and Namespace Name:" + type.Name + " / " + type.Namespace + " ***********");

                //todo: Games seems to stop working after changing type name or namespace name question is why?
                //type.Name = "TypeName_" + typesIndex.ToString();
                //Console.WriteLine("Type renamed to: " + type.Name.ToString());
                //type.Namespace = "NamespaceName_" + typesIndex.ToString();
                //Console.WriteLine("Namespace renamed to: " + type.Namespace);

                typesIndex++;

                foreach(var field in type.Fields)
                {
                    if(field.Access == FieldAttributes.Private)
                    {
                        Console.WriteLine("Changing field named:" + field.Name);
                        field.Name = "fieldName_" + fieldIndex.ToString();
                        fieldIndex++;
                    }
                }
                fieldIndex = 1;

                foreach (var method in type.Methods)
                {
                    Console.WriteLine("^^^^^^^^^^ Statring to preform changes on Method name: " + method.Name + " ^^^^^^^^^^");
                    foreach (var param in method.Parameters)
                    {
                        Console.WriteLine("Changing parameter named:" + param.Name);
                        param.Name = "ParamName_" + paramIndex.ToString();
                        paramIndex++;
                    }
                    paramIndex = 1;
                    if (IsMethodObfuscutable(method))
                    {
                        method.Name = "MethodName_" + methodsIndex.ToString();
                        methodsIndex++;
                        Console.WriteLine("Changed Method Name to: " + method.Name);

                        // local variables arent stored in .dll or .exe
                        //todo: Get PDB file and preform changes on it

                        //method.FreeMethodBody();
                        //foreach (var variable in method.Body.Variables)
                        //{
                        //    Console.WriteLine("Changing localvariable named: " + variable.Name);
                        //    variable.Name = "LocalVariable_" + variablesIndex.ToString();
                        //    variablesIndex++;
                        //}
                        //variablesIndex = 1;

                    }
                }
                methodsIndex = 1;
                Console.WriteLine("\n\n\n");
            }
           
        }

        //Check if method is private not abstract or overrides, not used in main processes and not inheritanced from monobeheviour
        bool IsMethodObfuscutable(MethodDef method)
        {
            if (method.Access != MethodAttributes.Private)
                return false;
            if (method.Name == ".cctor" || method.Name == ".ctor")
                return false;
            if (MonoBehaviourMethods.MonoBehaviourMethodsList.Any(e => e == method.Name.String))
                return false;
            if (method.IsAbstract == true)
                return false;
            if (method.HasOverrides == true)
                return false;


            return true;
        }
    }
}
