using dnlib.DotNet;

namespace CodeObfuscator
{
    // This program works in this way:
    // First it loads module (.exe or .dll file) from user
    // Then it preforms each Transform override method for this module and outputs the changed file in same folder with changed name
    // Module is used as parameter so it isnt changed on its own.

    class MainProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Give path to assembly that you want to obfuscate");
            string? modulePath = Console.ReadLine();
            if (modulePath == null)
                return;

            Obfuscator.ObfuscateFile(modulePath);
        }
    }
}


