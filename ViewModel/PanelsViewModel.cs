using System;
using System.Collections.Generic;
using System.Diagnostics;
using Caliburn.Micro;
using Models;
using Models.Rules;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Models.Log;
using MoreLinq;
using ViewModels.Editors;
using ViewModels.LineTransformers;
using ViewModels.Modules;
using ViewModels.Panels;
using ViewModels.Rules;
using ViewModels.Services;

namespace ViewModels
{
    public class PanelsViewModel : Conductor<IPanel>.Collection.AllActive
    {
        private readonly ILogState _logState;


        public PanelsViewModel(
            IPanel[] panels,
            ILogState logState)
        {
            _logState = logState;

            var rulesPanel = panels.First(p => p is RulesPanelViewModel);
            var allPanels = new IPanel[] { rulesPanel }.Concat(panels).DistinctBy(x => x.DisplayName);
            Items.AddRange(allPanels);
        }
        

        public async Task Filter() =>
            await _logState.Filter();


    }
}