using Mig;
using UnityEngine;

namespace RTG
{
#if ORIGINAL_RTG
    public interface IUndoRedoAction
    {
        void Execute();

        void Undo();
#else
    public interface IUndoRedoAction : IOperatorCommand
    {
#endif
        void Redo();
        void OnRemovedFromUndoRedoStack();
    }
}
