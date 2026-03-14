using Mig.UI.MainCavas;
using Mig.UI.Presentation;
using RTG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mig.UI
{
    public class UIController : JigSingleton<UIController>
    {
        public GameObject MainLoad;
        public MainCanvas MianCanvas;
        public GameObject ModelCanvas;
        public GameObject Plane;
        public GameObject RTGApp;
        public GameObject TranformationCanvas;
        public GameObject CinemChineCanvas;

        public PresentationViewController PresentationCanvas;

        public MigManager manager;

        public void SetEditorModeUI()
        {
            MainLoad.gameObject.SetActive(true);
            MianCanvas  .gameObject.SetActive(true);
            ModelCanvas .gameObject.SetActive(true);
            Plane       .gameObject.SetActive(true);
            RTGApp      .gameObject.SetActive(true);
            TranformationCanvas .gameObject.SetActive(true);
            CinemChineCanvas.gameObject.SetActive(true);

            PresentationCanvas.gameObject.SetActive(false);
        }

        public void SetPresentModeUI()
        {
            MainLoad.gameObject.SetActive(false);
            MianCanvas.gameObject.SetActive(false);
            ModelCanvas.gameObject.SetActive(false);
            Plane.gameObject.SetActive(false);
            RTGApp.gameObject.SetActive(false);
            TranformationCanvas.gameObject.SetActive(false);
            CinemChineCanvas.gameObject.SetActive(false);
            PresentationCanvas.gameObject.SetActive(true);
        }

        public void ShowLoadingModeUI()
        {
            // TODO show loading Ui
        }


        public void SetEnterPresentationTrigger(Action enterPresentationTrigger)
        {
            MianCanvas.SetEnterPresentationTrigger(enterPresentationTrigger);
        }

        public void SetExitPresentationTrigger (Action exitTrigger)
        {
            PresentationCanvas.OnExitPresentationCallback = exitTrigger;
        }
    }
}
