using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Interfaces.Data.Model.MetaData;
using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Interfaces.Data.Model.Relations;
using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.Utils;

namespace Dsmviz.Viewer.Data.Import.Dsi
{
    public class DsiModel(IModelEditing modelEditing, IModelQuery modelQuery) : IDsiModelFileCallback
    {
        private readonly IMetaDataModelEditing _metaDataModelEditing = modelEditing.MetaDataModelEditing;
        private readonly IElementModelEditing _elementModelEditing = modelEditing.ElementModelEditing;
        private readonly IRelationModelEditing _relationModelEditing = modelEditing.RelationModelEditing;

        private readonly IElementModelQuery _elementModelQuery = modelQuery.ElementModelQuery;
        private readonly IRelationModelQuery _relationModelQuery = modelQuery.RelationModelQuery;

        private readonly Dictionary<int, int> _dsiIdToDsmIdMapping = new();

        private readonly List<int> _notImportedElementIds = [];
        private readonly List<int> _notImportedRelationIds = [];

        public bool ImportModel(string dsiFilename, bool clearModel, IFileProgress progress)
        {
            Logger.LogDataModelMessage($"Import data model file={dsiFilename}");

            if (clearModel)
            {
                _metaDataModelEditing.Clear();
                _elementModelEditing.Clear();
                _relationModelEditing.Clear();
            }
            else
            {
                foreach (IElement existingElement in _elementModelQuery.GetElements())
                {
                    _notImportedElementIds.Add(existingElement.Id);
                }

                foreach (IRelation existingRelations in _relationModelQuery.GetRelations())
                {
                    _notImportedRelationIds.Add(existingRelations.Id);
                }
            }

            DsiModelFile dsiModelFile = new DsiModelFile(dsiFilename, this);
            bool success = dsiModelFile.Load(progress);

            if (success)
            {
                CleanupNoLongerExistingElements();
                CleanupNoLongerExistingRelations();

                _elementModelEditing.AssignElementOrder();
            }

            return success;
        }

        public IMetaDataItem ImportMetaDataItem(string group, string name, string value)
        {
            return _metaDataModelEditing.AddMetaDataItem(group, name, value);
        }

        public IElement? ImportElement(int id, string name, string type, IDictionary<string, string>? properties)
        {
            IElement? element = null;
            IElement? parent = null;

            char[] trimCharacters = [' ', '.'];
            string fullName = name.TrimStart(trimCharacters);
            ElementName elementName = new ElementName();
            foreach (string namePart in new ElementName(fullName).NameParts)
            {
                elementName.AddNamePart(namePart);

                bool isElementLeaf = fullName == elementName.FullName;

                if (isElementLeaf)
                {
                    element = AddElement(namePart, type, parent, properties);
                    if (element != null)
                    {
                        parent = element;
                        _dsiIdToDsmIdMapping[id] = element.Id;

                        Logger.LogInfo($"Import leaf element dsiId={id} dsmId={element.Id} dsmName={namePart}");
                    }
                }
                else
                {
                    element = AddElement(namePart, "", parent, null);
                    if (element != null)
                    {
                        parent = element;

                        Logger.LogInfo($"Import non leaf element dsiId={id} dsmId={element.Id} dsmName={namePart}");
                    }
                }
            }

            return element;
        }

        public IRelation? ImportRelation(int consumerId, int providerId, string type, int weight, IDictionary<string, string>? properties)
        {
            IRelation? relation = null;

            Logger.LogInfo($"Import relation consumerId={consumerId} providerId={providerId} type={type} weight={weight}");

            if (_dsiIdToDsmIdMapping.ContainsKey(consumerId) && _dsiIdToDsmIdMapping.TryGetValue(providerId, out var dsmProviderId))
            {
                int dsmConsumerId = _dsiIdToDsmIdMapping[consumerId];

                if (dsmConsumerId != dsmProviderId)
                {
                    IElement? consumer = _elementModelQuery.GetElementById(dsmConsumerId);
                    IElement? provider = _elementModelQuery.GetElementById(dsmProviderId);

                    if (consumer != null && provider != null)
                    {
                        relation = AddRelation(consumer, provider, type, weight, properties);
                    }
                    else
                    {
                        Logger.LogError($"Could not find consumer or provider of relation consumer={consumerId} provider={providerId}");
                    }
                }
            }
            else
            {
                Logger.LogError($"Could not find consumer or provider of relation consumer={consumerId} provider={providerId}");
            }

            return relation;
        }

        private IElement? AddElement(string name, string type, IElement? parent, IDictionary<string, string>? properties)
        {
            IElement? element = _elementModelQuery.GetElementByFullname(GetFullName(name, parent));
            if (element != null)
            {
                _notImportedElementIds.Remove(element.Id);
            }
            else
            {
                element = _elementModelEditing.AddElement(name, type, parent?.Id, 0, properties);
            }

            return element;
        }

        private IRelation? AddRelation(IElement consumer, IElement provider, string type, int weight, IDictionary<string, string>? properties)
        {
            IRelation? relation = _relationModelQuery.FindRelation(consumer, provider, type, weight); // Check if already exists
            if (relation != null)
            {
                _notImportedRelationIds.Remove(relation.Id);
            }
            else
            {
                relation = _relationModelEditing.AddRelation(consumer, provider, type, weight, properties);
            }
            return relation;
        }

        private void CleanupNoLongerExistingRelations()
        {
            foreach (int relationId in _notImportedRelationIds)
            {
                _relationModelEditing.RemoveRelation(relationId);
            }
        }

        private void CleanupNoLongerExistingElements()
        {
            foreach (int elementId in _notImportedElementIds)
            {
                _elementModelEditing.RemoveElement(elementId);
            }
        }

        private static string GetFullName(string name, IElement? parent)
        {
            string fullName;
            if (parent != null)
            {
                return parent.Fullname + "." + name;
            }
            else
            {
                fullName = name;
            }

            return fullName;
        }
    }
}
