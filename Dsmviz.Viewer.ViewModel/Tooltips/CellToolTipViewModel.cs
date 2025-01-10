using System.ComponentModel;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.ViewModel.Tooltips;
using Dsmviz.Viewer.ViewModel.Common;

namespace Dsmviz.Viewer.ViewModel.Tooltips
{
    public class CellToolTipViewModel(IElement consumer, IElement provider) : ViewModelBase, ICellToolTipViewModel
    {
        public string Title { get; } = "Cell";
        public int ConsumerId { get; } = consumer.Id;
        public string ConsumerName { get; } = consumer.Fullname;
        public int ProviderId { get; } = provider.Id;
        public string ProviderName { get; } = provider.Fullname;
    }
}

