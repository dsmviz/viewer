using System.ComponentModel;

namespace Dsmviz.Interfaces.ViewModel.Tooltips
{
    public interface ICellToolTipViewModel
    {
        string Title { get; }
        int ConsumerId { get; }
        string ConsumerName { get; }
        int ProviderId { get; }
        string ProviderName { get; }
        event PropertyChangedEventHandler? PropertyChanged;
    }
}
