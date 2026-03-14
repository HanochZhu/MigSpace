using RTG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig
{
    public class OperatorCommandManager : EasySington<OperatorCommandManager>
    {

        private Stack<IOperatorCommand> m_ExecutedCommand = new();

        public void Execute(IOperatorCommand command)
        {
            if(command is not IUndoRedoAction)
            {
                // IUndoRedoAction is already execute outside.
                command.Execute();
            }
            m_ExecutedCommand.Push(command);
        }

        public void Undo()
        {
            if (m_ExecutedCommand.Count == 0)
            {
                return;
            }
            var latestCommand = m_ExecutedCommand.Pop();

            latestCommand.Undo();
        }
    }

}
