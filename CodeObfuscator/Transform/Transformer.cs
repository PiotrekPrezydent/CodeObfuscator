using dnlib.DotNet;

namespace CodeObfuscator.Transform
{
    //base class for all Transformers, every Transformer should inheritance from this class, check TransformManager or any Transformer for more info
    internal abstract class Transformer
    {
        public Transformer(ModuleDefMD modulesParam, string nameParam, TransformType typeParam)
        {
            Name = nameParam;
            Type = typeParam;
            Module = modulesParam;
        }
        public ModuleDefMD Module { get; set; }
        public string Name { get; set; }
        public TransformType Type { get; set; }

        public abstract void Transform(ModuleDefMD module);

    }
    public enum TransformType
    {
        Rename,
        Flow,
        Other
    }
}
