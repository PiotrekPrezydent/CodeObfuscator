using dnlib.DotNet;

namespace CodeObfuscator.Transform
{
    // Simple manager for all transform that should be preformed on module, when creating new Transform it must be added in constructor to main transformers list
    internal class TransformerManager
    {
        public TransformerManager(ModuleDefMD moduleBase)
        {
            _transformers.Add(new RenameTransformer(moduleBase));
        }
        List<Transformer> _transformers = new List<Transformer>();
        public List<Transformer> GetTransformers() => _transformers;

    }
}
