using Mig.Core;
using Mig.Snapshot;
using Mig.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig
{

    public class MigPresentationController : IMigStateController
    {
        private UIController uiController;

        public MigPresentationController(UIController _uiController, Action onExitPresentation)
        {
            uiController = _uiController;
            uiController.SetExitPresentationTrigger(onExitPresentation);
        }
        public void Awake()
        {

            // start from 0
            SnapshotManager.Instance.ApplyToTargetSnapshot(0);
            uiController.SetPresentModeUI();
        }

        public void Sleep()
        {

        }

        public void Update()
        {

        }

        public void UpdateUI()
        {

        }
    }

}
