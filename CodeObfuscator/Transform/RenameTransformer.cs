using CodeObfuscator.Consts;
using dnlib.DotNet;

namespace CodeObfuscator.Transform
{
    internal sealed class RenameTransformer : Transformer
    {
        const int minCharacters = 8;
        const int maxCharacters = 32;
        const int minASCI = 65;
        const int maxASCI = 90;
        List<string> generatedNames;

        public RenameTransformer(ModuleDefMD module) : base(module,"Renamer", TransformType.Rename)
        {
            this.Module = module;
        }

        //Preforms Name transform on module (.exe or .dll file) for valid methods, fields and parameters
        public override void Transform(ModuleDefMD module)
        {
            string oldName;
            generatedNames = new List<string>();

            foreach (var type in module.Types)
            {
                Console.WriteLine("**************** Statring to preform changes on Type and Namespace Name:" + type.Name + " / " + type.Namespace + " ****************");

                //todo: Probably we can change names of only those types and namespaces that dont inheritance from MonoBehaviour,
                //need more samples for checking

                Console.WriteLine("\n");
                Console.WriteLine("######## Changing Fields ########");

                foreach (var field in type.Fields)
                {
                    if (field.HasCustomAttributes)
                        continue;

                    if(field.Access == FieldAttributes.Private)
                    {
                        oldName = field.Name;
                        field.Name = GenerateNewName();
                        Console.WriteLine("Changed field name: " + oldName + " To: " + field.Name);
                    }

                }

                Console.WriteLine("\n");
                Console.WriteLine("######## Changing Methods ########");

                foreach (var method in type.Methods)
                {
                    if (method.HasCustomAttributes)
                        continue;
                    Console.WriteLine("^^^^^^^^ Statring to preform changes on Method named: " + method.Name + " ^^^^^^^^");

                    if (IsMethodObfuscutable(method))
                    {
                        oldName = method.Name;
                        method.Name = GenerateNewName();
                        Console.WriteLine("Changed Method name: " + oldName + " To: " + method.Name);
                    }

                    Console.WriteLine("\n");

                    foreach (var param in method.Parameters)
                    {
                        if (param.Name == "")
                            continue;
                        oldName = param.Name;
                        param.Name = GenerateNewName();
                        Console.WriteLine("Changed Parameter name: " + oldName + " To: " + param.Name);
                    }
                }
                Console.WriteLine("\n\n\n");
            }
           
        }

        //Check if method is private, not used in main processes and dont inheritance from monobeheviour
        bool IsMethodObfuscutable(MethodDef method)
        {
            if (method.Access != MethodAttributes.Private)
                return false;
            if (method.Name == ".cctor" || method.Name == ".ctor")
                return false;
            if (Constants.MonoBehaviourMethodsList.Any(e => e == method.Name.String))
                return false;

            return true;
        }
        string GenerateNewName()
        {
            string newName = "";
            while (generatedNames.Contains(newName) || newName == "")
            {
                Random rnd = new Random();
                int numberOfCharacters = rnd.Next(minCharacters, maxCharacters + 1);
                newName = "";
                for (int i = 0; i < numberOfCharacters; i++)
                {
                    Random randomAsciLetter = new Random();
                    int x = randomAsciLetter.Next(minASCI, maxASCI + 1);
                    char letter = (char)x;
                    newName += letter.ToString();
                }
            }
            generatedNames.Add(newName);
            return newName;
        }

    }
}
