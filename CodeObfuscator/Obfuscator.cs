using dnlib.DotNet;

namespace CodeObfuscator
{
    public static class Obfuscator
    {
        public static void ObfuscateFile(string mainPath)
        {
            ModuleDefMD baseModule;
            baseModule = ModuleDefMD.Load(mainPath);
            SplitPath(mainPath, out string fileNameWithPath, out string fileExtension);
            baseModule.Write(fileNameWithPath + "-Temp" + fileExtension);
            //baseModule.Dispose();
            //File.Delete(mainPath);
            //File.Move(fileNameWithPath + "-Temp" + fileExtension, mainPath);

            void SplitPath(string FilePath, out string fileNameWithPath, out string fileExtension)
            {
                int dotIndex = FilePath.LastIndexOf(".");
                fileNameWithPath = FilePath.Substring(0, dotIndex);
                fileExtension = "." + FilePath.Substring(dotIndex + 1);
            }
        }

        static void RenameMethodsAndFields(ModuleDefMD module)
        {
            string oldName;

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

                    if (field.Access == FieldAttributes.Private)
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
                    oldName = method.Name;
                    method.Name = GenerateNewName();
                    Console.WriteLine("Changed Method name: " + oldName + " To: " + method.Name);

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

            string GenerateNewName()
            {
                const int minCharacters = 8;
                const int maxCharacters = 32;
                const int minASCI = 65;
                const int maxASCI = 90;
                string newName = "";
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
                return newName;
            }
        }
    }
}
