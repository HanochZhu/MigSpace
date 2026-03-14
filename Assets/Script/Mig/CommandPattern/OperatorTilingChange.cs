using Mig;
using Mig.Core;
using Mig.Snapshot;
using System.Collections;
using System.Collections.Generic;
using TriLibCore.Dae.Schema;
using UnityEngine;

public class OperatorTilingChange : IOperatorCommand
{
    private MigTilingElement m_TilingElement;
    private MigMaterial m_material;

    private Vector2 m_tarfetTiling;
    private Vector2 m_srcTiling;

    public OperatorTilingChange(MigMaterial material, Vector2 targetTiling)
    {
        m_material = material;
        m_tarfetTiling = targetTiling;
    }
    public void Execute()
    {
        m_TilingElement = MigElementManager.GetOrAddCurrentStepElement<MigTilingElement>(m_material.host);

        m_TilingElement.CurrentTiling = m_tarfetTiling;
        m_TilingElement.OperateCount++;

        m_srcTiling = m_material.mainTextureScale;
        m_material.mainTextureScale = m_tarfetTiling;
        SnapshotManager.Instance.UpdateCurrentSnapShot();

    }

    public void Undo()
    {
        m_material.mainTextureScale = m_srcTiling;

        if (m_TilingElement != null)
        {
            m_TilingElement.CurrentTiling = m_srcTiling;
            m_TilingElement.OperateCount--;
            if (m_TilingElement.OperateCount == 0)
            {
                GameObject.Destroy(m_TilingElement.Wrapper);
            }
        }
    }
}
