using Dsmviz.Interfaces.Application.Query;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Dsmviz.Viewer.ViewModel.Lists.Element
{
    public class ElementListViewModel : ViewModelBase
    {
        public ElementListViewModel(ElementListViewModelType viewModelType, IRelationQuery relationQuery, IElement selectedConsumer, IElement selectedProvider)
        {
            Title = "Element List";

            IEnumerable<IElement> elements;
            switch (viewModelType)
            {
                case ElementListViewModelType.RelationConsumers:
                    SubTitle = $"Consumers in relations from:\n -{selectedConsumer.Fullname}\nto:\n -{selectedProvider.Fullname}";
                    elements = relationQuery.GetRelationConsumers(selectedConsumer, selectedProvider);
                    break;
                case ElementListViewModelType.RelationProviders:
                    SubTitle = $"Providers in relations from:\n -{selectedConsumer.Fullname}\nto:\n -{selectedProvider.Fullname}";
                    elements = relationQuery.GetRelationProviders(selectedConsumer, selectedProvider);
                    break;
                case ElementListViewModelType.ElementConsumers:
                    SubTitle = $"Consumers of:\n -{selectedProvider.Fullname}";
                    elements = relationQuery.GetElementConsumers(selectedProvider);
                    break;
                case ElementListViewModelType.ElementProvidedInterface:
                    SubTitle = $"Provided interface of:\n -{selectedProvider.Fullname}";
                    elements = relationQuery.GetElementInterface(selectedProvider);
                    break;
                case ElementListViewModelType.ElementRequiredInterface:
                    SubTitle = $"Required interface of:\n -{selectedProvider.Fullname}";
                    elements = relationQuery.GetElementProviders(selectedProvider);
                    break;
                default:
                    SubTitle = "";
                    elements = new List<IElement>();
                    break;
            }

            List<ElementListItemViewModel> elementViewModels = [];

            foreach (IElement element in elements)
            {
                elementViewModels.Add(new ElementListItemViewModel(element));
            }

            elementViewModels.Sort();

            int index = 1;
            foreach (ElementListItemViewModel viewModel in elementViewModels)
            {
                viewModel.Index = index;
                index++;
            }

            Elements = new ObservableCollection<ElementListItemViewModel>(elementViewModels);

            CopyToClipboardCommand = RegisterCommand(CopyToClipboardExecute);
        }

        public string Title { get; }
        public string SubTitle { get; }

        public ObservableCollection<ElementListItemViewModel> Elements { get; }

        public ICommand CopyToClipboardCommand { get; }

        private void CopyToClipboardExecute(object? parameter)
        {
            if (Elements.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                StringBuilder headerLine = new StringBuilder();
                headerLine.Append("Index,");
                headerLine.Append("Path,");
                headerLine.Append("Name,");
                headerLine.Append("Type,");
                builder.AppendLine(headerLine.ToString());

                foreach (ElementListItemViewModel viewModel in Elements)
                {
                    StringBuilder line = new StringBuilder();
                    line.Append($"{viewModel.Index},");
                    line.Append($"{viewModel.ElementPath},");
                    line.Append($"{viewModel.ElementName},");
                    line.Append($"{viewModel.ElementType},");
                    builder.AppendLine(line.ToString());
                }
                // TODO FIX: Clipboard.SetText(builder.ToString());
            }
        }
    }
}
