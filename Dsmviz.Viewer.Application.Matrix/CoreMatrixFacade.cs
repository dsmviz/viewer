

using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Data.Model.Relations;

namespace Dsmviz.Viewer.Application.Matrix
{
    public class CoreMatrixFacade : IMatrix
    {
        public IDependencyWeightMatrix DependencyWeightMatrix { get; }
        public IDependencyCycleMatrix DependencyCycleMatrix { get; }

        public CoreMatrixFacade(IRelationModelQuery relationModelQuery)
        {
            DependencyWeightMatrix = new DependencyWeightMatrix(relationModelQuery);
            DependencyCycleMatrix = new DependencyCycleMatrix(DependencyWeightMatrix);
        }
    }
}
