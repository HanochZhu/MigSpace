using Mig.Core;
using Mig.Snapshot;
using RTG;
using System.Collections.Generic;
using UnityEngine;

namespace Mig
{
    public class OperatorTranslateCommand : IOperatorCommand
    {
        private Transform m_OperateGO;
        // world spaced
        private List<LocalTransformSnapshot> _preChangeTransformSnapshots = new List<LocalTransformSnapshot>();
        private List<LocalTransformSnapshot> _postChangeTransformSnapshots = new List<LocalTransformSnapshot>();
        /// <summary>
        /// Maybe we can operate go at respective element object.
        /// </summary>
        private List<MigTranslateElement> m_elements;

        public OperatorTranslateCommand(List<LocalTransformSnapshot> preChangeTransformSnapshots,
                                        List<LocalTransformSnapshot> postChangeTransformSnapshots)
        {
            _preChangeTransformSnapshots = new List<LocalTransformSnapshot>(preChangeTransformSnapshots);
            _postChangeTransformSnapshots = new List<LocalTransformSnapshot>(postChangeTransformSnapshots);

            m_elements = new();
        }

        public void Execute()
        {
            foreach (LocalTransformSnapshot changeTrans in _postChangeTransformSnapshots)
            {
                var element = changeTrans.Transform.gameObject.GetOrAddCurrentStepElement<MigTranslateElement>();

                element.StepLocalPosition = changeTrans.LocalPosition;
                element.StepLocalRotation = changeTrans.LocalRotation;
                element.StepLocalScale = changeTrans.LocalScale;

                element.OperateCount ++;

                m_elements.Add(element);
            }

            SnapshotManager.Instance.UpdateCurrentSnapShot();
        }

        public void Undo()
        {
            Debug.Assert(m_elements.Count == _preChangeTransformSnapshots.Count, $"[Mig] count of element {m_elements.Count} is not equal to _preChangeTransformSnapshots.Count {_preChangeTransformSnapshots.Count}");

            for (int i = 0; i < m_elements.Count; i++)
            {
                var element = m_elements[i];
                if (element == null)
                {
                    continue;
                }
                // TODO we might remove element. if all status return to begin.

                element.StepLocalPosition = _preChangeTransformSnapshots[i].LocalPosition;
                element.StepLocalRotation = _preChangeTransformSnapshots[i].LocalRotation;
                element.StepLocalScale = _preChangeTransformSnapshots[i].LocalScale;


                element.OperateCount--;
                if (element.OperateCount == 0)
                {
                    element.Wrapper.RemoveElement(element);
                    element.Wrapper = null;
                    element = null;
                }
            }
        }
    }
}

