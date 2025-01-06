using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Viewer.Application.Editing.Action.Management;


namespace Dsmviz.Viewer.Application.Editing.Facade
{
    public class CoreEditingFacade : IEditing
    {
        public CoreEditingFacade(IDataModel dataModel, ISortAlgorithms sortAlgorithms, IDependencyWeightMatrix matrix)
        {
            ActionManager = new ActionManager();

            ActionManagement = new ActionManagement(ActionManager);
            ElementEditing = new ElementEditing(ActionManager, dataModel, sortAlgorithms, matrix);
            RelationEditing = new RelationEditing(ActionManager, dataModel);
        }

        public IActionManagement ActionManagement { get; }
        public IElementEditing ElementEditing { get; }
        public IRelationEditing RelationEditing { get; }

        protected ActionManager ActionManager { get; }

    }
}
