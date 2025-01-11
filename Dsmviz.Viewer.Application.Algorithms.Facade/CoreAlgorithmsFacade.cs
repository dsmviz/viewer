using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Viewer.Application.Algorithms.Alphabetic;
using Dsmviz.Viewer.Application.Algorithms.Partition;

namespace Dsmviz.Viewer.Application.Algorithms.Facade
{
    public class CoreAlgorithmsFacade : ISortAlgorithms
    {
        private static readonly Dictionary<string, Type> Algorithms;

        static CoreAlgorithmsFacade()
        {
            Algorithms = new Dictionary<string, Type>();
            RegisterAlgorithmTypes();
        }

        public static void RegisterAlgorithm(string name, Type algorithm)
        {
            Algorithms[name] = algorithm;
        }

        public ISortAlgorithm? CreateAlgorithm(string algorithmName)
        {
            ISortAlgorithm? algorithm = null;

            if (Algorithms.TryGetValue(algorithmName, out var type))
            {
                algorithm = Activator.CreateInstance(type) as ISortAlgorithm;
            }
            return algorithm;
        }

        public IEnumerable<string> GetSupportedAlgorithms()
        {
            return Algorithms.Keys;
        }

        private static void RegisterAlgorithmTypes()
        {
            RegisterAlgorithm(PartitionSortAlgorithm.AlgorithmName, typeof(PartitionSortAlgorithm));
            RegisterAlgorithm(AlphabeticalAscendingSortAlgorithm.AlgorithmName, typeof(AlphabeticalAscendingSortAlgorithm));
            RegisterAlgorithm(AlphabeticalDescendingSortAlgorithm.AlgorithmName, typeof(AlphabeticalDescendingSortAlgorithm));
        }
    }
}
