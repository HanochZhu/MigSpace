using Mig;
using Mig.Core;
using System;
using UnityEngine;
namespace Mig
{
    public class OperatorMaterialChange : IOperatorCommand
    {
        private MigMaterialElement m_MaterialElement;
        private Renderer m_renderer;
        private Guid m_targetMaterialGUID;
        private Guid m_srcMaterialGUID;
        private Material OriginMaterial;
        public OperatorMaterialChange(Renderer _renderer, Guid matGUID)
        {
            this.m_renderer = _renderer;

            m_MaterialElement = MigElementManager.GetOrAddCurrentStepElement<MigMaterialElement>(_renderer.gameObject);

            // TODO, support more color channel

            if (m_MaterialElement.CurrentMaterialGUID == Guid.Empty)
            {
                OriginMaterial = this.m_renderer.material;
            }
            m_MaterialElement.CurrentMaterialGUID = matGUID;

            m_targetMaterialGUID = matGUID;
        }
        public void Execute()
        {
            m_MaterialElement.CurrentMaterialGUID = m_targetMaterialGUID;
            m_MaterialElement.OperateCount++;
            m_MaterialElement.Apply();
        }

        public void Undo()
        {
            if (m_MaterialElement != null)
            {
                m_MaterialElement.CurrentMaterialGUID = m_srcMaterialGUID;
                m_MaterialElement.OperateCount--;
                m_MaterialElement.Apply();
                if (m_MaterialElement.OperateCount == 0)
                {
                    GameObject.Destroy(m_MaterialElement.Wrapper);
                    m_renderer.material = OriginMaterial;
                }
            }
        }
    }
}
