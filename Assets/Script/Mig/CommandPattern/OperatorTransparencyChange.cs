using Mig;
using Mig.Core;
using Mig.Snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorTransparencyChange : IOperatorCommand
{
    private MigTransparencyElement m_TransparencyElement;
    private MigMaterial material;

    private float m_tarfetTransparency;
    private float m_srcTransparency;

    public OperatorTransparencyChange(MigMaterial _material, float tarfetTransparency)
    {
        material = _material;
        m_tarfetTransparency = tarfetTransparency;
    }
    public void Execute()
    {
        m_TransparencyElement = MigElementManager.GetOrAddCurrentStepElement<MigTransparencyElement>(material.host);

        m_TransparencyElement.CurrentTransparency = m_tarfetTransparency;
        m_TransparencyElement.OperateCount++;

        m_srcTransparency = material.mainColor.a;

        var color = material.mainColor;
        color.a = m_tarfetTransparency;
        material.mainColor = color;
        SnapshotManager.Instance.UpdateCurrentSnapShot();

    }

    public void Undo()
    {
        var color = material.mainColor;
        color.a = m_srcTransparency;
        material.mainColor = color;

        if (m_TransparencyElement != null)
        {
            m_TransparencyElement.CurrentTransparency = m_srcTransparency;
            m_TransparencyElement.OperateCount--;
            if (m_TransparencyElement.OperateCount == 0)
            {
                GameObject.Destroy(m_TransparencyElement.Wrapper);
            }
        }
    }
}
