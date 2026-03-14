using Mig.Core;
using Mig.Snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig
{
    public class OperatorColorChange : IOperatorCommand
    {
        private MigColorElement m_ColorElement;
        private MigMaterial m_material;
        private Color m_targetColor;
        private Color m_srcColor;
        public OperatorColorChange(MigMaterial mat, Color color)
        {
            // TODO, support more color channel
            this.m_material = mat;
            
            m_targetColor = color;
        }

        public void Execute()
        {
            m_ColorElement = m_material.host.GetOrAddCurrentStepElement<MigColorElement>();
            if (m_ColorElement == null)
                Debug.Log("m_ColorElement is null");
            m_ColorElement.CurrentMaterialColor = m_targetColor;
            m_ColorElement.OperateCount++;


            m_srcColor = m_material.mainColor;
            m_material.mainColor = m_targetColor;
            SnapshotManager.Instance.UpdateCurrentSnapShot();

        }

        public void Undo()
        {
            m_material.mainColor = m_srcColor;

            // TODO: This piece of code should be encapsulate in a util
            if (m_ColorElement != null)
            {
                m_ColorElement.CurrentMaterialColor = m_srcColor;
                m_ColorElement.OperateCount--;
                if (m_ColorElement.OperateCount == 0)
                {
                    GameObject.Destroy(m_ColorElement.Wrapper);
                }
            }
        }
    }

}

