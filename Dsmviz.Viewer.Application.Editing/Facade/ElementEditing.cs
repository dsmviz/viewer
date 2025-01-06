

using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Interfaces.Application.Editing;
using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Data.Entities;
using Dsmviz.Interfaces.Data.Model.Elements;
using Dsmviz.Interfaces.Data.Model.Model;
using Dsmviz.Viewer.Application.Editing.Action.Element;
using Dsmviz.Viewer.Application.Editing.Action.Management;
using Dsmviz.Viewer.Application.Editing.Data;

namespace Dsmviz.Viewer.Application.Editing.Facade
{
    public class ElementEditing(
        IActionManager actionManager,
        IDataModel dataModel,
        ISortAlgorithms sortAlgorithms,
        IDependencyWeightMatrix matrix)
        : IElementEditing
    {
        private readonly IElementModelQuery _elementModelQuery = dataModel.ElementModelQuery;
        private readonly IElementModelEditing _elementModelEditing = dataModel.ElementModelEditing;

        public void Sort(IElement element, string algorithm)
        {
            IElementWeightMatrix weightsMatrix = new ElementWeightsMatrix(matrix, element);
            ISortAlgorithm? sortAlgorithm = sortAlgorithms.CreateAlgorithm(algorithm);
            if (sortAlgorithm != null)
            {
                ElementSortAction action = new ElementSortAction(_elementModelEditing, element, weightsMatrix, sortAlgorithm);
                actionManager.Execute(action);
            }
        }

        public IEnumerable<string> GetSupportedSortAlgorithms()
        {
            return sortAlgorithms.GetSupportedAlgorithms();
        }

        public IElement? NextSibling(IElement element)
        {
            return _elementModelEditing.NextSibling(element);
        }

        public IElement? PreviousSibling(IElement element)
        {
            return _elementModelEditing.PreviousSibling(element);
        }

        public bool IsElementOnClipboard()
        {
            return actionManager.GetContext().IsElementOnClipboard();
        }

        public void MoveUp(IElement element)
        {
            ElementMoveUpAction action = new ElementMoveUpAction(_elementModelEditing, element);
            actionManager.Execute(action);
        }

        public void MoveDown(IElement element)
        {
            ElementMoveDownAction action = new ElementMoveDownAction(_elementModelEditing, element);
            actionManager.Execute(action);
        }

        public IEnumerable<string> GetElementTypes()
        {
            return _elementModelQuery.GetElementTypes();
        }

        public IElement? GetElementByFullname(string text)
        {
            return _elementModelQuery.GetElementByFullname(text);
        }

        public IElement? CreateElement(string name, string type, IElement parent, int index)
        {
            ElementCreateAction action = new ElementCreateAction(_elementModelEditing, name, type, parent, index);
            return actionManager.Execute(action) as IElement;
        }

        public void DeleteElement(IElement element)
        {
            ElementDeleteAction action = new ElementDeleteAction(_elementModelEditing, element);
            actionManager.Execute(action);
        }

        public void ChangeElementName(IElement element, string name)
        {
            ElementChangeNameAction action = new ElementChangeNameAction(_elementModelEditing, element, name);
            actionManager.Execute(action);
        }

        public void ChangeElementType(IElement element, string type)
        {
            ElementChangeTypeAction action = new ElementChangeTypeAction(_elementModelEditing, element, type);
            actionManager.Execute(action);
        }

        public void ChangeElementParent(IElement element, IElement newParent, int index)
        {
            if (_elementModelEditing.IsChangeElementParentAllowed(element, newParent))
            {
                ElementChangeParentAction action = new ElementChangeParentAction(_elementModelEditing, element, newParent, index);
                actionManager.Execute(action);
            }
        }

        public void CutElement(IElement element)
        {
            ElementCutAction action = new ElementCutAction(_elementModelEditing, element, actionManager.GetContext());
            actionManager.Execute(action);
        }

        public void CopyElement(IElement element)
        {
            ElementCopyAction action = new ElementCopyAction(element, actionManager.GetContext());
            actionManager.Execute(action);
        }

        public void PasteElement(IElement newParent, int index)
        {
            ElementPasteAction action = new ElementPasteAction(_elementModelEditing, newParent, index, actionManager.GetContext());
            actionManager.Execute(action);
        }
    }
}
