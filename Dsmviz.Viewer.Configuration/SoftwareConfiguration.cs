
using Dsmviz.Viewer.Application.Algorithms.Facade;
using Dsmviz.Viewer.Application.Editing.Facade;
using Dsmviz.Viewer.Application.Matrix;
using Dsmviz.Viewer.Application.Metrics;
using Dsmviz.Viewer.Application.Persistency;
using Dsmviz.Viewer.Application.Query;
using Dsmviz.Viewer.Data.Model.Facade;
using Dsmviz.Viewer.Data.Store;
using Dsmviz.Viewer.ViewModel.Main;

namespace Dsmviz.Viewer.Configuration
{
    public class SoftwareConfiguration
    {
        public static MainViewModel CreateMainViewModel()
        {
            CoreDataModel model = new CoreDataModel();
            CoreDataStore store = new CoreDataStore(model);
            CoreAlgorithmsFacade algorithms = new CoreAlgorithmsFacade();
            CoreMatrixFacade matrix = new CoreMatrixFacade(model.RelationModelQuery);
            CoreEditingFacade editing = new CoreEditingFacade(model, algorithms, matrix.DependencyWeightMatrix);
            CoreQueryFacade query = new CoreQueryFacade(model);
            CoreMetricsFacade metrics = new CoreMetricsFacade(model, matrix.DependencyCycleMatrix);
            CorePersistencyFacade persistency = new CorePersistencyFacade(store, model, editing.ActionManagement);

            return new MainViewModel("Dsmviz", editing, query, matrix, metrics, persistency);
        }
    }
}