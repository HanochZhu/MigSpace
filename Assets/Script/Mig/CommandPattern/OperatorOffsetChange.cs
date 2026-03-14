using Mig;
using Mig.Core;
using Mig.Snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorOffsetChange : IOperatorCommand
{
    private MigOffsetElement m_OffsetElement;
    private MigMaterial m_material;

    private Vector2 m_tarfetOffset;
    private Vector2 m_srcOffset;

    public OperatorOffsetChange(MigMaterial material, Vector2 tarfetOffset)
    {
        m_material = material;
        m_tarfetOffset = tarfetOffset;
    }
    public void Execute()
    {
        m_OffsetElement = MigElementManager.GetOrAddCurrentStepElement<MigOffsetElement>(m_material.host);

        m_OffsetElement.CurrentOffset = m_tarfetOffset;
        m_OffsetElement.OperateCount++;

        m_srcOffset = m_material.mainTextureOffset;
        m_material.mainTextureOffset = m_tarfetOffset;
        SnapshotManager.Instance.UpdateCurrentSnapShot();

    }

    public void Undo()
    {
        m_material.mainTextureOffset = m_srcOffset;

        if (m_OffsetElement != null)
        {
            m_OffsetElement.CurrentOffset = m_srcOffset;
            m_OffsetElement.OperateCount--;
            if (m_OffsetElement.OperateCount == 0)
            {
                GameObject.Destroy(m_OffsetElement.Wrapper);
            }
        }
    }
}
