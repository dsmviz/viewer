using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Viewer.ViewModel.Common;
using System.Text;
using System.Windows.Input;

namespace Dsmviz.Viewer.ViewModel.Lists.Action
{
    public class ActionListViewModel : ViewModelBase
    {
        private readonly IActionManagement _actionManagement;
        private IEnumerable<ActionListItemViewModel> _actions = [];

        public ActionListViewModel(IActionManagement actionManagement)
        {
            Title = "Edit history";

            _actionManagement = actionManagement;
            _actionManagement.ActionPerformed += OnActionPerformed;

            UpdateActionList();

            CopyToClipboardCommand = RegisterCommand(CopyToClipboardExecute);
            ClearCommand = RegisterCommand(ClearExecute);
        }

        private void OnActionPerformed(object? sender, EventArgs e)
        {
            UpdateActionList();
        }

        public string Title { get; }

        public IEnumerable<ActionListItemViewModel> Actions
        {
            get => _actions;
            set { _actions = value; OnPropertyChanged(); }
        }

        public ICommand CopyToClipboardCommand { get; }
        public ICommand ClearCommand { get; }

        private void CopyToClipboardExecute(object? parameter)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ActionListItemViewModel viewModel in Actions)
            {
                builder.AppendLine($"{viewModel.Index,-5}, {viewModel.Action,-30}, {viewModel.Details}");
            }
            // TODO FIX: Clipboard.SetText(builder.ToString());
        }

        private void ClearExecute(object? parameter)
        {
            _actionManagement.ClearActions();
            UpdateActionList();
        }

        private void UpdateActionList()
        {
            var actionViewModels = new List<ActionListItemViewModel>();
            int index = 1;
            foreach (IAction action in _actionManagement.GetActions())
            {
                actionViewModels.Add(new ActionListItemViewModel(index, action));
                index++;
            }

            Actions = actionViewModels;
        }
    }
}
