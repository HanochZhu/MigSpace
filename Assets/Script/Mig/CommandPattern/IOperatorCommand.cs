using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig
{
    public interface IOperatorCommand
    {
        void Execute();

        void Undo();

    }

}
