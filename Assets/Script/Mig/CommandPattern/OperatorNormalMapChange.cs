using Mig;
using Mig.Core;
using Mig.Snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorNormalMapChange : IOperatorCommand
{
    private MigNormalMapElement m_NormalMapElement;
    private Renderer m_renderer;

    private Texture m_tarfetNormalMap;
    private Texture m_srcNormalMap;

    public OperatorNormalMapChange(Renderer _renderer, Texture2D tarfetNormalMap)
    {
        m_renderer = _renderer;
        m_tarfetNormalMap = tarfetNormalMap;
    }
    public void Execute()
    {
        m_NormalMapElement = MigElementManager.GetOrAddCurrentStepElement<MigNormalMapElement>(m_renderer.gameObject);

        m_NormalMapElement.CurrentNormalMap = m_tarfetNormalMap;
        m_NormalMapElement.OperateCount++;

        m_srcNormalMap = m_renderer.material.GetTexture("_BumpMap");
        m_renderer.material.SetTexture("_BumpMap", m_tarfetNormalMap);
        SnapshotManager.Instance.UpdateCurrentSnapShot();

    }

    public void Undo()
    {
        m_renderer.material.SetTexture("_BumpMap", m_srcNormalMap);
        if (m_NormalMapElement != null)
        {
            m_NormalMapElement.CurrentNormalMap = m_srcNormalMap;
            m_NormalMapElement.OperateCount--;
            if (m_NormalMapElement.OperateCount == 0)
            {
                GameObject.Destroy(m_NormalMapElement.Wrapper);
            }
        }
    }
}
