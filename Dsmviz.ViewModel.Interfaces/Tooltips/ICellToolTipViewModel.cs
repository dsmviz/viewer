using System.ComponentModel;

namespace Dsmviz.ViewModel.Interfaces.Tooltips
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
