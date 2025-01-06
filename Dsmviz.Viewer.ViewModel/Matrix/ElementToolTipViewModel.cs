

using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Viewer.ViewModel.Common;

namespace Dsmviz.Viewer.ViewModel.Matrix
{
    public class ElementToolTipViewModel(IElement element) : ViewModelBase
    {
        public string Title => "Element";
        public int Id => element.Id;
        public string Name => element.Fullname;
    }
}
