using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Rules;
using Models.Settings;

namespace ViewModels.Configuration
{
    public class ConfigurationData
    {
        public Uri NetworkRulesProviderUri { get; set; }
        public string SyntaxHighlighting { get; set; }
        public IRule DefaultRule { get; set; }
        public StringCollection FavoriteLibraryRules { get; set; }

        public bool Fold { get; set; }

        public ParserType ParserType { get; set; }
    }
}
