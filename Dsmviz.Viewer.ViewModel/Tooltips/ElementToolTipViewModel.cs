using System.ComponentModel;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.ViewModel.Tooltips;
using Dsmviz.Viewer.ViewModel.Common;

namespace Dsmviz.Viewer.ViewModel.Tooltips
{
    public class ElementToolTipViewModel(IElement element) : ViewModelBase, IElementToolTipViewModel
    {
        public string Title => "Element";
        public int Id => element.Id;
        public string Name => element.Fullname;
        public string Type => element.Type;
    }
}
