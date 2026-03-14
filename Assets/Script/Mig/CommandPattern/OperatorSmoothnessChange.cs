using Mig;
using Mig.Core;
using Mig.Snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorSmoothnessChange : IOperatorCommand
{
    private MigSmoothnessElement m_SmoothnessElement;
    private MigMaterial _mat;

    private float m_tarfetSmoothness;
    private float m_srcSmoothness;

    public OperatorSmoothnessChange(MigMaterial _material, float tarfetSmoothness)
    {
        _mat = _material;
        m_tarfetSmoothness = tarfetSmoothness;
    }
    public void Execute()
    {
        m_SmoothnessElement = MigElementManager.GetOrAddCurrentStepElement<MigSmoothnessElement>(_mat.host);

        m_SmoothnessElement.CurrentSmoothness = m_tarfetSmoothness;
        m_SmoothnessElement.OperateCount++;

        m_srcSmoothness = _mat.Smoothness;
        _mat.Smoothness = m_tarfetSmoothness;
        SnapshotManager.Instance.UpdateCurrentSnapShot();

    }

    public void Undo()
    {
        _mat.Smoothness = m_srcSmoothness;

        if (m_SmoothnessElement != null)
        {
            m_SmoothnessElement.CurrentSmoothness = m_srcSmoothness;
            m_SmoothnessElement.OperateCount--;
            if (m_SmoothnessElement.OperateCount == 0)
            {
                GameObject.Destroy(m_SmoothnessElement.Wrapper);
            }
        }
    }
}
