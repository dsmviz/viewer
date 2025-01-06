using Dsmviz.Interfaces.Util;
using Dsmviz.Viewer.Utils;
using System.Xml;

namespace Dsmviz.Viewer.Data.Import.Dsi
{
    public class DsiModelFile(string filename, IDsiModelFileCallback modelFileCallback)
    {
        private const string RootXmlNode = "dsimodel";
        private const string ModelElementCountXmlAttribute = "elementCount";
        private const string ModelRelationCountXmlAttribute = "relationCount";

        private const string MetaDataGroupXmlNode = "metadatagroup";
        private const string MetaDataGroupNameXmlAttribute = "name";
        private const string MetaDataXmlNode = "metadata";
        private const string MetaDataItemNameXmlAttribute = "name";
        private const string MetaDataItemValueXmlAttribute = "value";

        private const string ElementXmlNode = "element";
        private const string ElementIdXmlAttribute = "id";
        private const string ElementNameXmlAttribute = "name";
        private const string ElementTypeXmlAttribute = "type";

        private const string RelationXmlNode = "relation";
        private const string RelationFromXmlAttribute = "from";
        private const string RelationToXmlAttribute = "to";
        private const string RelationTypeXmlAttribute = "type";
        private const string RelationWeightXmlAttribute = "weight";

        private int _totalElementCount;
        private int _progressedElementCount;
        private int _totalRelationCount;
        private int _progressedRelationCount;

        public bool Load(IFileProgress progress)
        {
            try
            {
                progress.ReportStart("Loading dsi model", "items");
                CompressedFile modelFile = new CompressedFile(filename);
                modelFile.ReadFile(ReadDsiXml, progress);
                progress.ReportDone();
                return true;
            }
            catch (Exception e)
            {
                progress.ReportError($"Exception {e.Message} during load dsi model {filename}");
                return false;
            }
        }

        private void ReadDsiXml(Stream stream, IFileProgress progress)
        {
            XmlReader xReader = XmlReader.Create(stream);
            while (xReader.Read())
            {
                switch (xReader.NodeType)
                {
                    case XmlNodeType.Element:
                        ReadModelAttributes(xReader);
                        ReadMetaDataGroup(xReader);
                        ReadElement(xReader, progress);
                        ReadRelation(xReader, progress);
                        break;
                    case XmlNodeType.Text:
                        break;
                    case XmlNodeType.EndElement:
                        break;
                }
            }
        }

        private void ReadModelAttributes(XmlReader xReader)
        {
            if (xReader.Name == RootXmlNode)
            {
                int? elementCount = ParseInt(xReader.GetAttribute(ModelElementCountXmlAttribute));
                int? relationCount = ParseInt(xReader.GetAttribute(ModelRelationCountXmlAttribute));

                if (elementCount.HasValue && relationCount.HasValue)
                {
                    _totalElementCount = elementCount.Value;
                    _progressedElementCount = 0;
                    _totalRelationCount = relationCount.Value;
                    _progressedRelationCount = 0;
                }
            }
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
                                modelFileCallback.ImportMetaDataItem(group, name, value);
                            }
                        }
                    }
                }
            }
        }

        private void ReadElement(XmlReader xReader, IFileProgress progress)
        {
            if (xReader.Name == ElementXmlNode)
            {
                int? id = null;
                string name = "";
                string type = "";

                Dictionary<string, string> elementProperties = new Dictionary<string, string>();
                for (int attInd = 0; attInd < xReader.AttributeCount; attInd++)
                {
                    xReader.MoveToAttribute(attInd);
                    switch (xReader.Name)
                    {
                        case ElementIdXmlAttribute:
                            id = ParseInt(xReader.Value);
                            break;
                        case ElementNameXmlAttribute:
                            name = xReader.Value;
                            break;
                        case ElementTypeXmlAttribute:
                            type = xReader.Value;
                            break;
                        default:
                            if (!string.IsNullOrEmpty(xReader.Value))
                            {
                                elementProperties[xReader.Name] = xReader.Value;
                            }
                            break;
                    }
                }

                if (id.HasValue)
                {
                    modelFileCallback.ImportElement(id.Value, name, type,
                        elementProperties.Count > 0 ? elementProperties : null);
                }

                _progressedElementCount++;
                progress.ReportProgress(GetTotalItemCount(), GetProgressedItemCount());
            }
        }

        private void ReadRelation(XmlReader xReader, IFileProgress progress)
        {
            if (xReader.Name == RelationXmlNode)
            {
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

                if (consumerId.HasValue && providerId.HasValue && weight.HasValue)
                {
                    modelFileCallback.ImportRelation(consumerId.Value, providerId.Value, type, weight.Value,
                        relationProperties.Count > 0 ? relationProperties : null);
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

        private int? ParseInt(string? value)
        {
            int? result = null;

            if (int.TryParse(value, out var parsedValued))
            {
                result = parsedValued;
            }
            return result;
        }
    }
}
