using CodeObfuscator.Transform;
using dnlib.DotNet;

namespace CodeObfuscator
{
    // This program works in this way:
    // First it loads module (.exe or .dll file) from user
    // Then it preforms each Transform override method for this module and outputs the changed file in same folder with changed name
    // Module is used as parameter so it isnt changed on its own.

    class MainProgram
    {
        static TransformerManager? _managerInstance;
        static void Main(string[] args)
        {
            Console.WriteLine("Give path to assembly that you want to obfuscate");
            string? modulePath = Console.ReadLine();
            if (modulePath == null)
                return;

            ObfuscateFile(modulePath);
        }

        static void ObfuscateFile(string mainPath)
        {
            ModuleDefMD baseModule;
            baseModule = ModuleDefMD.Load(mainPath);
            _managerInstance = new TransformerManager(baseModule);
            _managerInstance.GetTransformers().ForEach(t => t.Transform(baseModule));
            SplitPath(mainPath, out string fileNameWithPath, out string fileExtension);
            baseModule.Write(fileNameWithPath+"-Temp"+fileExtension);
            //baseModule.Dispose();
            //File.Delete(mainPath);
            //File.Move(fileNameWithPath + "-Temp" + fileExtension, mainPath);
        }

        static void SplitPath(string FilePath,out string fileNameWithPath,out string fileExtension)
        {
            int dotIndex = FilePath.LastIndexOf(".");
            fileNameWithPath = FilePath.Substring(0,dotIndex);
            fileExtension = "."+FilePath.Substring(dotIndex + 1);
        }
    }
}


