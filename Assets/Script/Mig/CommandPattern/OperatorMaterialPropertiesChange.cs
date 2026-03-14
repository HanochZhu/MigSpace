using Mig;
using Mig.Snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class OperatorMaterialPropertiesChange : IOperatorCommand
{
    private Renderer m_renderer;

    private float m_tarfetMetallic;
    private float m_tarfetSmoothness;
    private Vector2 m_tarfetTiling;
    private Vector2 m_tarfetOffset;
    private float m_tarfetTransparency;


    private float m_srcMetallic;
    private float m_srcSmoothness;
    private Vector2 m_srcTiling;
    private Vector2 m_srcOffset;
    private float m_srcTransparency;

    public OperatorMaterialPropertiesChange(Renderer _renderer, float m_tarfetMetallic, float m_tarfetSmoothness,
    Vector2 m_tarfetTiling, Vector2 m_tarfetOffset, float m_tarfetTransparency)
    {
        this.m_renderer = _renderer;
        if (m_tarfetMetallic == 0)
            m_tarfetMetallic = m_renderer.material.GetFloat("_Metallic");
        else
            this.m_tarfetMetallic = m_tarfetMetallic;
        if (m_tarfetSmoothness == 0)
            m_tarfetSmoothness = m_renderer.material.GetFloat("_Glossiness");
        else
            this.m_tarfetSmoothness = m_tarfetSmoothness;
        if (m_tarfetTiling == Vector2.zero)
            m_tarfetTiling = m_renderer.material.mainTextureScale;
        else
            this.m_tarfetTiling = m_tarfetTiling;
        if (m_tarfetOffset == Vector2.zero)
            m_tarfetOffset = m_renderer.material.mainTextureOffset;
        else
            this.m_tarfetOffset = m_tarfetOffset;
        if (m_tarfetTransparency == 0)
            m_tarfetTransparency = m_renderer.material.color.a;
        else
            this.m_tarfetTransparency = m_tarfetTransparency;
    }
    public void Execute()
    {
        m_srcMetallic = m_tarfetMetallic;
        //Debug.Log("m_srcMetallic" + m_srcMetallic);
        m_srcSmoothness = m_tarfetSmoothness;
        m_srcTiling = m_tarfetTiling;
        m_srcOffset = m_tarfetOffset;
        m_srcTransparency = m_tarfetTransparency;

        m_renderer.material.SetFloat("_Metallic", m_tarfetMetallic);
        m_renderer.material.SetFloat("_Glossiness", m_tarfetSmoothness);
        m_renderer.material.mainTextureScale = m_tarfetTiling;
        m_renderer.material.mainTextureOffset = m_tarfetOffset;

        var color = m_renderer.material.color;
        color.a = m_tarfetTransparency;
        m_renderer.material.color = color;
        SnapshotManager.Instance.UpdateCurrentSnapShot();

    }

    public void Undo()
    {
        Debug.Log("Undo m_srcMetallic" + m_srcMetallic);
        m_renderer.material.SetFloat("_Metallic", m_srcMetallic);
        m_renderer.material.SetFloat("_Glossiness", m_srcSmoothness);
        m_renderer.material.mainTextureScale = m_srcTiling;
        m_renderer.material.mainTextureOffset = m_srcOffset;

        var color = m_renderer.material.color;
        color.a = m_srcTransparency;
        m_renderer.material.color = color;
    }


}
