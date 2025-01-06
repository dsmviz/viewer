namespace Dsmviz.Interfaces.Application.Matrix
{
    public interface IMatrix
    {
        IDependencyWeightMatrix DependencyWeightMatrix { get; }
        IDependencyCycleMatrix DependencyCycleMatrix { get; }
    }
}
