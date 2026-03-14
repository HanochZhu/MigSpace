using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mig.UI.Notification
{
    public class MigNotificationUIController : MonoBehaviour
    {

        public MigNotificationSaveWindow saveWindow;
        public MigNotificationLoadingWindow loadTip;

        private void OnEnable()
        {
            EventManager.StartListening(MigEventCommon.OnSaveModelBegin, OnModelSaveBegin);
            EventManager.StartListening(MigEventCommon.OnSaveModelEnd, OnModelSaveComplete);

            EventManager.StartListening(MigEventCommon.OnLoadingModelBegin, OnLoadingModelBegin);
            EventManager.StartListening(MigEventCommon.OnLoadingModelEnd, OnLoadingModelEnd);
        }



        private void OnDisable()
        {
            EventManager.StopListening(MigEventCommon.OnSaveModelEnd, OnModelSaveBegin);
            EventManager.StopListening(MigEventCommon.OnSaveModelBegin, OnModelSaveComplete);

            EventManager.StopListening(MigEventCommon.OnLoadingModelBegin, OnLoadingModelBegin);
            EventManager.StopListening(MigEventCommon.OnLoadingModelEnd, OnLoadingModelEnd);
        }

        void Start()
        {
            saveWindow.saveCloseTipButton.onClick.AddListener(Close);
        }

        private void Close()
        {
            saveWindow.gameObject.SetActive(false);
        }

        private void OnModelSaveBegin(object arg0, object arg1)
        {
            loadTip.gameObject.SetActive(true);
            loadTip.content.text = (string)arg0;
        }

        private void OnModelSaveComplete(object arg0, object arg1)
        {
            loadTip.gameObject.SetActive(false);
        }

        private void OnLoadingModelEnd(object arg0, object arg1)
        {
            loadTip.gameObject.SetActive(false);

        }

        private void OnLoadingModelBegin(object arg0, object arg1)
        {
            loadTip.gameObject.SetActive(true);
            loadTip.content.text = (string)arg0;
        }
    }

}
