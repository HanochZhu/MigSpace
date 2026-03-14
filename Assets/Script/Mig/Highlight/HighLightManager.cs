using HighlightPlus;
using UnityEngine;

namespace Mig
{
    public class HighLightManager
    {
        public HighlightProfile profile;
        private HighlightEffect m_currentOutLineModel;
        private string profileAssetsPath = "HighlightProfile";

        public void OnEnable()
        {
            EventManager.StartListening(MigEventCommon.OnSelectedChanged, OnSelectedChanged);
            profile = Resources.Load(profileAssetsPath) as HighlightProfile;
        }

        public void OnDisable()
        {
            EventManager.StopListening(MigEventCommon.OnSelectedChanged, OnSelectedChanged);
        }

        public void Destroy()
        {
            if (m_currentOutLineModel == null)
            {
                return;
            }
            DehighLightModel(m_currentOutLineModel.gameObject);
            m_currentOutLineModel = null;
        }

        private void OnSelectedChanged(object obj, object arg1)
        {
            if(obj == null)
            {
                if (m_currentOutLineModel)
                    DehighLightModel(m_currentOutLineModel.gameObject);
                return; 
            }
            GameObject selected = (GameObject)obj;
            if (m_currentOutLineModel != null)
            {
                DehighLightModel(m_currentOutLineModel.gameObject);
            }
            m_currentOutLineModel = selected.GetOrAddComponent<HighlightEffect>();
            UpdateHighlightProfile();
        }

        private void UpdateHighlightProfile() 
        {
            m_currentOutLineModel.profile = profile;
            m_currentOutLineModel.isSelected = true;
            m_currentOutLineModel.highlighted = true;
            m_currentOutLineModel.outlineWidth = profile.outlineWidth;
            m_currentOutLineModel.outlineColor = profile.outlineColor;
            m_currentOutLineModel.outlineQuality = profile.outlineQuality;
            m_currentOutLineModel.outlineVisibility = profile.outlineVisibility;
        }
        /// <summary>
        /// clean high light
        /// </summary>
        private void DehighLightModel(GameObject target)
        {
            var outline = target.GetComponent<HighlightEffect>();

            if (outline != null)
            {
                GameObject.DestroyImmediate(outline);
            }
            m_currentOutLineModel = null;
        }



    }
}
