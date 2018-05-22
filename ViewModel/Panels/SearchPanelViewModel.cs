using System.Linq;
using Caliburn.Micro;
using MoreLinq;
using ViewModels.LineTransformers;
using ViewModels.Modules;
using System;
using ViewModels.Services;

namespace ViewModels.Panels
{
    public class SearchPanelViewModel : PanelBase, ISearchService
    {
        private readonly SearchTransformer _searchTransformer;
        private readonly IVisualTransformers _visualTransformers;

        public SearchPanelViewModel(SearchTransformer searchTransformer, IVisualTransformers visualTransformers)
            : base("Search")
        {
            _searchTransformer = searchTransformer;
            _visualTransformers = visualTransformers;
            Criterions = new BindableCollection<SearchCriterionViewModel>
            {
                new SearchCriterionViewModel() {Text = ""},
            };
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _visualTransformers.Transformers.Add(_searchTransformer);
        }

        public BindableCollection<SearchCriterionViewModel> Criterions { get; }

        public void Delete(SearchCriterionViewModel searchCriterion)
        {
            if (Criterions.Count == 1)
            {
                Criterions[0].Text = "";
                return;
            }

            Criterions.Remove(searchCriterion);
            _searchTransformer.Update(Criterions);

            //begin hack
            _visualTransformers.Transformers.Remove(_searchTransformer);
            _visualTransformers.Transformers.Add(_searchTransformer);
            //end hack

        }

        public void Apply(SearchCriterionViewModel searchCriterion)
        {

            //begin hack - random crash
            if (searchCriterion == null)
                return;
            //end hack

            if (Criterions.IndexOf(searchCriterion) < Criterions.Count - 1 && string.IsNullOrEmpty(searchCriterion.Text))
            {
                Criterions.Remove(searchCriterion);
            }

            Refresh();

            var lastElement = Criterions.Last();
            if (!string.IsNullOrEmpty(lastElement.Text))
            {
                Criterions.Add(new SearchCriterionViewModel());
            }
        }

        private void Refresh()
        {

            _searchTransformer.Update(Criterions);

            //begin hack - refrsh visible rows
            _visualTransformers.Transformers.Remove(_searchTransformer);
            _visualTransformers.Transformers.Add(_searchTransformer);
            //end hack
        }

        public void AddCriterion(string text)
        {
            Criterions.Add(new SearchCriterionViewModel(text));
            Refresh();
        }
    }
}
