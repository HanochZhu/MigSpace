using Mig;
using Mig.Core;
using Mig.Snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorMainTextureChange : IOperatorCommand
{
    private MigMainTextureElement m_MainTextureElement;
    private Renderer m_renderer;

    private Texture2D m_tarfetTexture;
    private Texture2D m_srcTexture;

    public OperatorMainTextureChange(Renderer _renderer, Texture2D tarfetTexture)
    {
        m_renderer = _renderer;
        m_tarfetTexture = tarfetTexture;
    }
    public void Execute()
    {
        m_MainTextureElement = MigElementManager.GetOrAddCurrentStepElement<MigMainTextureElement>(m_renderer.gameObject);

        m_MainTextureElement.CurrentTexture = m_tarfetTexture;
        m_MainTextureElement.OperateCount++;

        m_srcTexture = (Texture2D)m_renderer.material.mainTexture;
        m_renderer.material.mainTexture = m_tarfetTexture;
        SnapshotManager.Instance.UpdateCurrentSnapShot();
    }

    public void Undo()
    {
        m_renderer.material.mainTexture = m_srcTexture;

        if (m_MainTextureElement != null)
        {
            m_MainTextureElement.CurrentTexture = m_srcTexture;
            m_MainTextureElement.OperateCount--;
            if (m_MainTextureElement.OperateCount == 0)
            {
                GameObject.Destroy(m_MainTextureElement.Wrapper);
            }
        }
    }
}
