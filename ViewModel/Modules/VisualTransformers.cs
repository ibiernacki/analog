using ICSharpCode.AvalonEdit.Rendering;

namespace ViewModels.Modules
{
    public class VisualTransformers : IVisualTransformers
    {
        public IReactiveCollection<IVisualLineTransformer> Transformers { get; } = new ReactiveCollection<IVisualLineTransformer>();
    }
}