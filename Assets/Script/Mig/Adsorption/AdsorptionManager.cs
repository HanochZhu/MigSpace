using UnityEngine;

public class AdsorptionManager
{
    private AdsorptionScript m_currentAdsorptionModel;
    public void OnEnable()
    {
        EventManager.StartListening(MigEventCommon.OnSelectedChanged, OnSelectedChanged);
    }

    public void OnDisable()
    {
        EventManager.StopListening(MigEventCommon.OnSelectedChanged, OnSelectedChanged);
    }

    public void Distroy()
    {
        if(m_currentAdsorptionModel == null)
        {
            return;
        }
        DestroyAdsorptionScript(m_currentAdsorptionModel.gameObject);
    }

    private void OnSelectedChanged(object arg0, object arg1)
    {
        if(arg0 == null) 
        {
            return;
        }
        AdsorptionModel((GameObject)arg0);
    }


    public void AdsorptionModel(GameObject target)
    {
        if (m_currentAdsorptionModel != null)
        {
            if (m_currentAdsorptionModel.gameObject == target)
            {
                return;
            }

            DestroyAdsorptionScript(m_currentAdsorptionModel.gameObject);
        }
        m_currentAdsorptionModel = target.GetOrAddComponent<AdsorptionScript>();
    }

    private void DestroyAdsorptionScript(GameObject taget)
    {
        var outline = taget.GetComponent<AdsorptionScript>();

        if (outline != null)
        {
            GameObject.DestroyImmediate(outline);
        }

        m_currentAdsorptionModel = null;
    }
}
