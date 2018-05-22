using ICSharpCode.AvalonEdit.Rendering;

namespace ViewModels.Modules
{
    public interface IVisualTransformers
    {
        IReactiveCollection<IVisualLineTransformer> Transformers { get; }
    }
}