using Caliburn.Micro;
using Models.Rules;

namespace ViewModels.Rules
{
    public class RuleInfoViewModel : PropertyChangedBase
    {
        public RuleInfo RuleInfo { get; }

        public RuleInfoViewModel(RuleInfo ruleInfo)
        {
            RuleInfo = ruleInfo;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange();
            }
        }

        private RuleStatus _status;
        public RuleStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyOfPropertyChange();
            }
        }
        public bool IsFavorite
        {
            get { return _status == RuleStatus.Favorite; }
            set
            {
                Status = value ? RuleStatus.Favorite : RuleStatus.Default;
                NotifyOfPropertyChange();
            }
        }


    }

}
