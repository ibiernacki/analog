using Caliburn.Micro;

namespace ViewModels.Panels
{
    public abstract class PanelBase : Screen, IPanel
    {
        private object _content;
        public object Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                NotifyOfPropertyChange();
            }
        }

        private readonly string _name;
        public override string DisplayName => _name;

        protected PanelBase(string name)
        {
            _name = name;
        }

    }
}