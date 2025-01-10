
using System.ComponentModel;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.ViewModel.Common;
using Dsmviz.Interfaces.ViewModel.Matrix;
using Dsmviz.Interfaces.ViewModel.Tooltips;
using Dsmviz.Viewer.ViewModel.Common;
using Dsmviz.Viewer.ViewModel.Tooltips;

namespace Dsmviz.Viewer.ViewModel.Matrix
{


    public class MatrixRowHeaderTreeItemViewModel : ViewModelBase, IMatrixRowHeaderTreeItemViewModel
    {
        private readonly IMatrixViewModel _viewModel;
        private readonly List<MatrixRowHeaderTreeItemViewModel> _children;
        private MatrixRowHeaderTreeItemViewModel? _parent;

        public MatrixRowHeaderTreeItemViewModel(IMatrixViewModel viewModel, IElement element, int depth)
        {
            _viewModel = viewModel;
            _children = [];
            _parent = null;
            Element = element;
            Depth = depth;

            ToolTipViewModel = new ElementToolTipViewModel(Element);
        }

        public IElementToolTipViewModel ToolTipViewModel { get; }
        public IElement Element { get; }

        public int Depth { get; }

        public int Id => Element.Id;
        public int Order => Element.Order;
        public bool IsConsumer { get; set; }
        public bool IsProvider { get; set; }
        public bool IsMatch => Element.IsMatch;
        public bool IsBookmarked => Element.IsBookmarked;
        public string Name => Element.IsRoot ? "Root" : Element.Name;

        public bool IsSearchActive => _viewModel.IsSearchActive;

        public bool IsExpandable => Element.HasChildren;

        public bool IsExpanded
        {
            get => Element.IsExpanded;
            set => Element.IsExpanded = value;
        }

        public IReadOnlyList<IMatrixRowHeaderTreeItemViewModel> Children => _children;

        public IMatrixRowHeaderTreeItemViewModel? Parent => _parent;

        public ViewPerspective SelectedViewPerspective => _viewModel.SelectedViewPerspective;

        public void ToggleElementExpanded()
        {
            if (IsExpandable)
            {
                Element.IsExpanded = !Element.IsExpanded;
            }
        }

        public void AddChild(IMatrixRowHeaderTreeItemViewModel viewModel)
        {
            if (viewModel is MatrixRowHeaderTreeItemViewModel v)
            {
                _children.Add(v);
                v._parent = this;
            }
        }

        public void ClearChildren()
        {
            foreach (MatrixRowHeaderTreeItemViewModel viewModel in _children)
            {
                viewModel._parent = null;
            }
            _children.Clear();
        }

        public int LeafElementCount
        {
            get
            {
                int count = 0;
                CountLeafElements(this, ref count);
                return count;
            }
        }

        private void CountLeafElements(IMatrixRowHeaderTreeItemViewModel viewModel, ref int count)
        {
            if (viewModel.Children.Count == 0)
            {
                count++;
            }
            else
            {
                foreach (IMatrixRowHeaderTreeItemViewModel child in viewModel.Children)
                {
                    CountLeafElements(child, ref count);
                }
            }
        }

    }
}
