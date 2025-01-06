using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Interfaces.Data.Model.MetaData;
using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Interfaces.Data.Model.Relations;
using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.Utils;
using System.Xml;

namespace Dsmviz.Viewer.Data.Store
{
    public class CoreDsmFile(string filename, IModelPersistency modelPersistency)
    {
        private const string RootXmlNode = "dsmmodel";
        private const string ModelVersionXmlAttribute = "versions";
        private const string ModelElementCountXmlAttribute = "elementCount";
        private const string ModelRelationCountXmlAttribute = "relationCount";

        private const string MetaDataGroupXmlNode = "metadatagroup";
        private const string MetaDataGroupNameXmlAttribute = "name";

        private const string MetaDataXmlNode = "metadata";
        private const string MetaDataItemNameXmlAttribute = "name";
        private const string MetaDataItemValueXmlAttribute = "value";

        private const string ElementGroupXmlNode = "elements";

        private const string ElementXmlNode = "element";
        private const string ElementIdXmlAttribute = "id";
        private const string ElementOrderXmlAttribute = "order";
        private const string ElementNameXmlAttribute = "name";
        private const string ElementTypeXmlAttribute = "type";
        private const string ElementExpandedXmlAttribute = "expanded";
        private const string ElementParentXmlAttribute = "parent";
        private const string ElementBookmarkedXmlAttribute = "bookmarked";

        private const string RelationGroupXmlNode = "relations";

        private const string RelationXmlNode = "relation";
        private const string RelationIdXmlAttribute = "id";
        private const string RelationFromXmlAttribute = "from";
        private const string RelationToXmlAttribute = "to";
        private const string RelationTypeXmlAttribute = "type";
        private const string RelationWeightXmlAttribute = "weight";

        private readonly IMetaDataModelPersistency _metaDataModelPersistency = modelPersistency.MetaDataModelPersistency;
        private readonly IElementModelPersistency _elementModelPersistency = modelPersistency.ElementModelPersistency;
        private readonly IRelationModelPersistency _relationModelPersistency = modelPersistency.RelationModelPersistency;

        private int _totalElementCount;
        private int _progressedElementCount;
        private int _totalRelationCount;
        private int _progressedRelationCount;

        public bool Save(bool compressed, IFileProgress progress)
        {
            try
            {
                progress.ReportStart("Saving dsm model", "items");
                CompressedFile modelFile = new CompressedFile(filename);
                modelFile.WriteFile(WriteDsmXml, progress, compressed);
                progress.ReportDone();
                return true;
            }
            catch (Exception e)
            {
                progress.ReportError($"Exception {e.Message} during saving dsm model {filename}");
                return false;
            }
        }

        public bool Load(IFileProgress progress)
        {
            try
            {
                progress.ReportStart("Loading dsm model", "items");
                CompressedFile modelFile = new CompressedFile(filename);
                modelFile.ReadFile(ReadDsmXml, progress);
                progress.ReportDone();
                return true;
            }
            catch (Exception e)
            {
                progress.ReportError($"Exception {e.Message} during to loading dsm model {filename}");
                return false;
            }
        }

        public bool IsCompressedFile
        {
            get
            {
                CompressedFile modelFile = new CompressedFile(filename);
                return modelFile.IsCompressed;
            }
        }

        private void WriteDsmXml(Stream stream, IFileProgress progress)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  "
            };

            using XmlWriter writer = XmlWriter.Create(stream, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement(RootXmlNode);
            {
                WriteItems(writer, progress);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        protected virtual void WriteItems(XmlWriter writer, IFileProgress progress)
        {
            WriteModelAttributes(writer);
            WriteMetaData(writer);
            WriteElements(writer, progress);
            WriteRelations(writer, progress);
        }

        private void ReadDsmXml(Stream stream, IFileProgress progress)
        {
            using XmlReader xReader = XmlReader.Create(stream);
            while (xReader.Read())
            {
                switch (xReader.NodeType)
                {
                    case XmlNodeType.Element:
                        ReadItem(xReader, progress);
                        break;
                    case XmlNodeType.Text:
                        break;
                    case XmlNodeType.EndElement:
                        break;
                }
            }
        }

        protected virtual void ReadItem(XmlReader xReader, IFileProgress progress)
        {
            ReadModelAttributes(xReader);
            ReadMetaDataGroup(xReader);
            ReadElement(xReader, progress);
            ReadRelation(xReader, progress);
        }

        protected virtual void WriteModelAttributes(XmlWriter writer)
        {
            modelPersistency.ModelVersion = modelPersistency.ModelVersion + 1;
            writer.WriteAttributeString(ModelVersionXmlAttribute, modelPersistency.ModelVersion.ToString());

            _totalElementCount = _elementModelPersistency.GetPersistedElementCount() - 1; // Root not written/read
            writer.WriteAttributeString(ModelElementCountXmlAttribute, _totalElementCount.ToString());
            _progressedElementCount = 0;

            _totalRelationCount = _relationModelPersistency.GetPersistedRelationCount();
            writer.WriteAttributeString(ModelRelationCountXmlAttribute, _totalRelationCount.ToString());
            _progressedRelationCount = 0;
        }

        protected virtual void ReadModelAttributes(XmlReader xReader)
        {
            if (xReader.Name == RootXmlNode)
            {
                int? modelVersion = ParseInt(xReader.GetAttribute(ModelVersionXmlAttribute));
                int? elementCount = ParseInt(xReader.GetAttribute(ModelElementCountXmlAttribute));
                int? relationCount = ParseInt(xReader.GetAttribute(ModelRelationCountXmlAttribute));

                modelPersistency.ModelVersion = modelVersion ?? 0;

                _totalElementCount = elementCount ?? 0;
                _progressedElementCount = 0;
                _totalRelationCount = relationCount ?? 0;
                _progressedRelationCount = 0;
            }
        }

        private void WriteMetaData(XmlWriter writer)
        {
            foreach (string group in _metaDataModelPersistency.GetMetaDataGroups())
            {
                WriteMetaDataGroup(writer, group);
            }
        }

        private void WriteMetaDataGroup(XmlWriter writer, string group)
        {
            writer.WriteStartElement(MetaDataGroupXmlNode);
            writer.WriteAttributeString(MetaDataGroupNameXmlAttribute, group);

            foreach (IMetaDataItem metaDataItem in _metaDataModelPersistency.GetMetaDataGroupItems(group))
            {
                writer.WriteStartElement(MetaDataXmlNode);
                writer.WriteAttributeString(MetaDataItemNameXmlAttribute, metaDataItem.Name);
                writer.WriteAttributeString(MetaDataItemValueXmlAttribute, metaDataItem.Value);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private void ReadMetaDataGroup(XmlReader xReader)
        {
            if (xReader.Name == MetaDataGroupXmlNode)
            {
                string? group = xReader.GetAttribute(MetaDataGroupNameXmlAttribute);
                if (group != null)
                {
                    XmlReader xMetaDataReader = xReader.ReadSubtree();
                    while (xMetaDataReader.Read())
                    {
                        if (xMetaDataReader.Name == MetaDataXmlNode)
                        {
                            string? name = xMetaDataReader.GetAttribute(MetaDataItemNameXmlAttribute);
                            string? value = xMetaDataReader.GetAttribute(MetaDataItemValueXmlAttribute);
                            if (name != null && value != null)
                            {
                                _metaDataModelPersistency.ImportMetaDataItem(group, name, value);
                            }
                        }
                    }
                }
            }
        }

        private void WriteElements(XmlWriter writer, IFileProgress progress)
        {
            writer.WriteStartElement(ElementGroupXmlNode);
            foreach (IElement element in _elementModelPersistency.GetRootElement().PersistedChildren)
            {
                WriteElement(writer, element, progress);
            }
            writer.WriteEndElement();
        }

        private void WriteElement(XmlWriter writer, IElement element, IFileProgress progress)
        {
            writer.WriteStartElement(ElementXmlNode);
            writer.WriteAttributeString(ElementIdXmlAttribute, element.Id.ToString());
            writer.WriteAttributeString(ElementOrderXmlAttribute, element.Order.ToString());
            writer.WriteAttributeString(ElementNameXmlAttribute, element.Name);
            writer.WriteAttributeString(ElementTypeXmlAttribute, element.Type);
            writer.WriteAttributeString(ElementExpandedXmlAttribute, element.IsExpanded.ToString());
            if (element.IsBookmarked)
            {
                writer.WriteAttributeString(ElementBookmarkedXmlAttribute, true.ToString());
            }
            if (element.Parent is { Id: > 0 })
            {
                writer.WriteAttributeString(ElementParentXmlAttribute, element.Parent.Id.ToString());
            }
            if (element.Properties != null)
            {
                foreach (KeyValuePair<string, string> elementProperty in element.Properties)
                {
                    writer.WriteAttributeString(elementProperty.Key, elementProperty.Value);
                }
            }
            writer.WriteEndElement();

            _progressedElementCount++;
            progress.ReportProgress(GetTotalItemCount(), GetProgressedItemCount());

            foreach (IElement child in element.PersistedChildren)
            {
                WriteElement(writer, child, progress);
            }
        }

        private void ReadElement(XmlReader xReader, IFileProgress progress)
        {
            if (xReader.Name == ElementXmlNode)
            {
                int? id = null;
                int? order = null;
                string name = "";
                string type = "";
                bool expanded = false;
                int? parent = null;
                bool bookmarked = false;

                Dictionary<string, string> elementProperties = new Dictionary<string, string>();
                for (int attInd = 0; attInd < xReader.AttributeCount; attInd++)
                {
                    xReader.MoveToAttribute(attInd);
                    switch (xReader.Name)
                    {
                        case ElementIdXmlAttribute:
                            id = ParseInt(xReader.Value);
                            break;
                        case ElementOrderXmlAttribute:
                            order = ParseInt(xReader.Value);
                            break;
                        case ElementNameXmlAttribute:
                            name = xReader.Value;
                            break;
                        case ElementTypeXmlAttribute:
                            type = xReader.Value;
                            break;
                        case ElementExpandedXmlAttribute:
                            expanded = ParseBool(xReader.Value);
                            break;
                        case ElementParentXmlAttribute:
                            parent = ParseInt(xReader.Value);
                            break;
                        case ElementBookmarkedXmlAttribute:
                            bookmarked = ParseBool(xReader.Value);
                            break;
                        default:
                            if (!string.IsNullOrEmpty(xReader.Value))
                            {
                                elementProperties[xReader.Name] = xReader.Value;
                            }
                            break;
                    }
                }

                if (id.HasValue && order.HasValue)
                {
                    _elementModelPersistency.ImportElement(id.Value,
                        name,
                        type,
                        elementProperties.Count > 0 ? elementProperties : null,
                        order.Value,
                        expanded,
                        bookmarked,
                        parent);
                }

                _progressedElementCount++;
                progress.ReportProgress(GetTotalItemCount(), GetProgressedItemCount());
            }
        }

        private void WriteRelations(XmlWriter writer, IFileProgress progress)
        {
            writer.WriteStartElement(RelationGroupXmlNode);
            foreach (IRelation relation in _relationModelPersistency.GetPersistedRelations())
            {
                WriteRelation(writer, relation, progress);
            }
            writer.WriteEndElement();
        }

        private void WriteRelation(XmlWriter writer, IRelation relation, IFileProgress progress)
        {
            writer.WriteStartElement(RelationXmlNode);
            writer.WriteAttributeString(RelationIdXmlAttribute, relation.Id.ToString());
            writer.WriteAttributeString(RelationFromXmlAttribute, relation.Consumer.Id.ToString());
            writer.WriteAttributeString(RelationToXmlAttribute, relation.Provider.Id.ToString());
            writer.WriteAttributeString(RelationTypeXmlAttribute, relation.Type);
            writer.WriteAttributeString(RelationWeightXmlAttribute, relation.Weight.ToString());
            if (relation.Properties != null)
            {
                foreach (KeyValuePair<string, string> relationProperty in relation.Properties)
                {
                    writer.WriteAttributeString(relationProperty.Key, relationProperty.Value);
                }
            }
            writer.WriteEndElement();

            _progressedRelationCount++;
            progress.ReportProgress(GetTotalItemCount(), GetProgressedItemCount());
        }

        private void ReadRelation(XmlReader xReader, IFileProgress progress)
        {
            if (xReader.Name == RelationXmlNode)
            {
                int? id = null;
                int? consumerId = null;
                int? providerId = null;
                string type = "";
                int? weight = null;

                Dictionary<string, string> relationProperties = new Dictionary<string, string>();
                for (int attInd = 0; attInd < xReader.AttributeCount; attInd++)
                {
                    xReader.MoveToAttribute(attInd);
                    switch (xReader.Name)
                    {
                        case RelationIdXmlAttribute:
                            id = ParseInt(xReader.Value);
                            break;
                        case RelationFromXmlAttribute:
                            consumerId = ParseInt(xReader.Value);
                            break;
                        case RelationToXmlAttribute:
                            providerId = ParseInt(xReader.Value);
                            break;
                        case RelationTypeXmlAttribute:
                            type = xReader.Value;
                            break;
                        case RelationWeightXmlAttribute:
                            weight = ParseInt(xReader.Value);
                            break;
                        default:
                            if (!string.IsNullOrEmpty(xReader.Value))
                            {
                                relationProperties[xReader.Name] = xReader.Value;
                            }
                            break;
                    }
                }

                if (id.HasValue && consumerId.HasValue && providerId.HasValue && weight.HasValue)
                {
                    IElement? consumer = _elementModelPersistency.GetElementById(consumerId.Value);
                    IElement? provider = _elementModelPersistency.GetElementById(providerId.Value);

                    if (consumer != null && provider != null)
                    {
                        _relationModelPersistency.ImportRelation(id.Value, consumer, provider, type, weight.Value,
                            relationProperties.Count > 0 ? relationProperties : null);
                    }
                    else
                    {
                        Logger.LogDataModelMessage("TODO3: Not found");
                    }
                }
                else
                {
                    Logger.LogDataModelMessage("TODO3: Not found");
                }
                _progressedRelationCount++;
                progress.ReportProgress(GetTotalItemCount(), GetProgressedItemCount());
            }
        }

        protected virtual int GetTotalItemCount()
        {
            return _totalElementCount + _totalRelationCount;
        }

        protected virtual int GetProgressedItemCount()
        {
            return _progressedElementCount + _progressedRelationCount;
        }

        protected int? ParseInt(string? value)
        {
            int? result = null;

            if (int.TryParse(value, out var parsedValued))
            {
                result = parsedValued;
            }
            return result;
        }

        protected bool ParseBool(string? value)
        {
            bool result = false;

            if (bool.TryParse(value, out var parsedValued))
            {
                result = parsedValued;
            }
            return result;
        }
    }
}
