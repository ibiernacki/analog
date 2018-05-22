using System.Windows;
using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using Models.Rules;
using ViewModels.Modules;
using ViewModels.Rules;

namespace ViewModels.Panels
{
    public class RulesPanelViewModel : PanelBase, IDropTarget
    {
        private readonly PropertiesPanelViewModel _propertiesPanelViewModel;
        private readonly IRules _rules;

        public RulesPanelViewModel(RuleViewModelFactory ruleViewModelFactory, PropertiesPanelViewModel propertiesPanelViewModel, IRules rules) 
            : base("Rules")
        {
            _propertiesPanelViewModel = propertiesPanelViewModel;
            _rules = rules;
            IsExpanded = true;
        }

        public BindableCollection<RuleViewModelBase> TreeRoot => _rules.TreeRoot;
        public RuleParentViewModelBase Root => _rules.Root;


        public void DeleteRule(RuleViewModelBase rule)
        {
            rule.Parent.Remove(rule);
        }

        protected override void OnActivate()
        {
            _propertiesPanelViewModel.ShowRuleProperties(Root);
        }

        public void ShowRuleProperties(RoutedPropertyChangedEventArgs<object> eArgs)
        {
            var rule = eArgs.NewValue as RuleViewModelBase;
            if (rule == null)
            {
                return;
            }

           _propertiesPanelViewModel.ShowRuleProperties(rule);
        }

        public void Delete(RuleViewModelBase rule)
        {
            //
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as RuleViewModelBase;
            var targetItem = dropInfo.TargetItem as RuleViewModelBase;


            if (sourceItem == null || targetItem == null)
            {
                return;
            }

            if (sourceItem.Parent == null)
            {
                return;
            }


            //drag parent into child
            if (sourceItem is RuleParentViewModelBase && ((RuleParentViewModelBase)sourceItem).IsParentOf(targetItem))
            {
                return;
            }

            //drag parent into self
            if (sourceItem is RuleParentViewModelBase && (dropInfo.InsertPosition & RelativeInsertPosition.TargetItemCenter) != 0 && sourceItem == targetItem)
            {
                return;
            }

            dropInfo.DropTargetAdorner = (targetItem is RuleParentViewModelBase && (dropInfo.InsertPosition & RelativeInsertPosition.TargetItemCenter) != 0)
            ? DropTargetAdorners.Highlight
            : DropTargetAdorners.Insert;

            dropInfo.Effects = DragDropEffects.Move;
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as RuleViewModelBase;
            var targetItem = dropInfo.TargetItem as RuleViewModelBase;

            if (targetItem is RuleParentViewModelBase &&
                (dropInfo.InsertPosition & RelativeInsertPosition.TargetItemCenter) != 0)
            {
                var parent = targetItem as RuleParentViewModelBase;
                parent.Add(sourceItem.Rule);
            }
            else
            {
                targetItem.Parent.Insert(dropInfo.InsertIndex, sourceItem.Rule);
            }

            sourceItem.Parent.Remove(sourceItem, false);

        }
    }
}
