
using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Dsmviz.ViewModel.Interfaces.Search;

namespace Dsmviz.Viewer.ViewModel.Search
{
    public class ElementSearchViewModel : ViewModelBase, IElementSearchViewModel
    {
        private readonly IElementQuery _elementQuery;
        private readonly IElement? _searchPathElement;
        private IElement? _selectedElement;
        private readonly bool _markMatchingElements;

        private string? _searchText;
        private bool _caseSensitiveSearch;
        private string? _selectedElementType;
        private ObservableCollection<string> _searchMatches = [];
        private SearchState _searchState;
        private string? _searchResult;

        public event EventHandler? SearchUpdated;

        public ElementSearchViewModel(IElementQuery elementQuery, IElement? searchPathElement, IElement? selectedElement, string? preSelectedElementType, bool markMatchingElements)
        {
            _elementQuery = elementQuery;
            _searchPathElement = searchPathElement ?? _elementQuery.RootElement;
            _selectedElement = selectedElement;
            _markMatchingElements = markMatchingElements;

            if (searchPathElement is { HasChildren: false })
            {
                _selectedElement = searchPathElement;
            }

            if (_selectedElement != null)
            {
                SearchText = _selectedElement.Fullname;
                SearchState = SearchState.Off;
            }
            else
            {
                SearchText = "";
                SearchState = SearchState.NoInput;
            }

            SearchPath = searchPathElement != null ? searchPathElement.Fullname : "";

            ElementTypes = [.. _elementQuery.GetElementTypes()];
            SelectedElementType = preSelectedElementType ?? string.Empty;

            SearchMatches = [];
            ClearSearchCommand = RegisterCommand(ClearSearchExecute, ClearSearchCanExecute);
        }

        public List<string> ElementTypes { get; }

        public ICommand ClearSearchCommand { get; }

        public string SearchPath
        {
            get;
        }

        public IElement? SelectedElement
        {
            get => _selectedElement;
            set { _selectedElement = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText ?? string.Empty;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    OnSearchTextUpdated();
                }
            }
        }

        public bool CaseSensitiveSearch
        {
            get => _caseSensitiveSearch;
            set
            {
                if (_caseSensitiveSearch != value)
                {
                    _caseSensitiveSearch = value;
                    OnPropertyChanged();
                    OnSearchTextUpdated();
                }
            }
        }

        public string SelectedElementType
        {
            get => _selectedElementType ?? string.Empty;
            set
            {
                if (_selectedElementType != value)
                {
                    _selectedElementType = value;
                    OnPropertyChanged();
                    OnSearchTextUpdated();
                }
            }
        }

        public ObservableCollection<string> SearchMatches
        {
            get => _searchMatches;
            private set { _searchMatches = value; OnPropertyChanged(); }
        }

        public SearchState SearchState
        {
            get => _searchState;
            set { _searchState = value; OnPropertyChanged(); }
        }

        public string SearchResult
        {
            get => _searchResult ?? string.Empty;
            set { _searchResult = value; OnPropertyChanged(); }
        }

        private void OnSearchTextUpdated()
        {
            if (SearchState != SearchState.Off)
            {
                IList<IElement> matchingElements = _elementQuery.SearchElements(SearchText, _searchPathElement, CaseSensitiveSearch, SelectedElementType, _markMatchingElements);

                List<string> matchingElementNames = [];
                foreach (IElement matchingElement in matchingElements)
                {
                    matchingElementNames.Add(matchingElement.Fullname);
                }
                SearchMatches = new ObservableCollection<string>(matchingElementNames);

                if (SearchText.Length == 0)
                {
                    SearchState = SearchState.NoInput;
                    SearchResult = "";
                    SelectedElement = null;
                }
                else if (SearchMatches.Count == 0)
                {
                    SearchState = SearchState.NoMatch;
                    SearchResult = SearchText.Length > 0 ? "None found" : "";
                    SelectedElement = null;
                }
                else
                {
                    SearchState = SearchState.Match;
                    SearchResult = $"{SearchMatches.Count} found";

                    if (SearchMatches.Count == 1)
                    {
                        SearchText = matchingElements[0].Fullname;
                    }
                }

                foreach (IElement matchingElement in matchingElements)
                {
                    if (SearchText == matchingElement.Fullname)
                    {
                        SelectedElement = matchingElement;
                    }
                }

                SearchUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ClearSearchExecute(object? parameter)
        {
            SearchText = "";
        }

        private bool ClearSearchCanExecute(object? parameter)
        {
            return SearchState != SearchState.Off;
        }
    }
}
