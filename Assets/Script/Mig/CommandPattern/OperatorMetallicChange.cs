using Mig;
using Mig.Core;
using Mig.Snapshot;
using UnityEngine;

public class OperatorMetallicChange : IOperatorCommand
{
    private MigMetallicElement m_MetallicElement;
    private MigMaterial m_materal;

    private float m_tarfetMetallic;
    private float m_srcMetallic;

    public OperatorMetallicChange(MigMaterial material, float tarfetMetallic)
    {
        m_materal = material;
        m_tarfetMetallic = tarfetMetallic;
    }
    public void Execute()
    {
        m_MetallicElement = MigElementManager.GetOrAddCurrentStepElement<MigMetallicElement>(m_materal.host);

        m_MetallicElement.CurrentMetallic = m_tarfetMetallic;
        m_MetallicElement.OperateCount++;

        m_srcMetallic = m_materal.Metallic;
        m_materal.Metallic = m_tarfetMetallic;
        SnapshotManager.Instance.UpdateCurrentSnapShot();

    }

    public void Undo()
    {
        m_materal.Metallic = m_srcMetallic;

        if (m_MetallicElement != null)
        {
            m_MetallicElement.CurrentMetallic = m_srcMetallic;
            m_MetallicElement.OperateCount--;
            if (m_MetallicElement.OperateCount == 0)
            {
                GameObject.Destroy(m_MetallicElement.Wrapper);
            }
        }
    }
}
