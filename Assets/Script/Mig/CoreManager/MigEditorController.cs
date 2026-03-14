using JigSpace;
using Mig.Core;
using Mig.Model;
using Mig.Snapshot;
using Mig.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace Mig
{
    public class MigEditorController : IMigStateController
    {
        private UIController controller;
        private UIController uIController;

        public List<Type> elementTypes;

        private HighLightManager highLightManager;
        private AdsorptionManager adsorptionManager;

        public MigEditorController(UIController uIController, Action OnExitEditorMode)
        {
            controller = uIController;
            uIController.SetEnterPresentationTrigger(OnExitEditorMode);

            Assembly assembly = Assembly.GetAssembly(typeof(MigElement));

            elementTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(MigElement).IsAssignableFrom(type))
                .ToList();
        }

        public void Awake()
        {
            Debug.LogWarning("Awake Editor controller");
            ModelManager.Instance.OnModelLoadCompleteEvent += onModelLoadComplete;
            controller.SetEditorModeUI();
            highLightManager = new HighLightManager();
            highLightManager.OnEnable();
            adsorptionManager = new AdsorptionManager();
            adsorptionManager.OnEnable();
        }

        public void Sleep()
        {
            Debug.LogWarning("Sleep Editor controller");

            if (controller == null || !ModelManager.Instance)
            {
                return;
            }

            highLightManager.OnDisable();
            highLightManager.Destroy();
            highLightManager = null;
            adsorptionManager.OnDisable();
            adsorptionManager.Distroy();
            adsorptionManager = null;
            ModelManager.Instance.OnModelLoadCompleteEvent -= onModelLoadComplete;
        }

        public void Update()
        {

        }

        public void UpdateUI()
        {

        }

        private void onModelLoadComplete()
        {
            // update snapshot first
            SnapshotManager.Instance.UpdateCurrentSnapShot();

            // add all default elment second
            var modelRoot = ModelManager.Instance.CurrentGameObjectRoot;

            Queue<Transform> bfs = new Queue<Transform>();
            bfs.Enqueue(modelRoot.transform);
            while (bfs.Count > 0)
            {
                var currentTrans = bfs.Dequeue();

                if (currentTrans.GetComponent<Renderer>() && currentTrans.GetComponent<MigElementWrapper>() == null)
                {
                    AddAllElementTypeToWrapper(currentTrans.AddComponent<MigElementWrapper>());
                }

                foreach (Transform trans in currentTrans)
                {
                    bfs.Enqueue(trans);
                }
            }
            SnapshotManager.Instance.AppleToTargetSnapshotWaitForFrame(0, null);
        }

        public void AddAllElementTypeToWrapper(MigElementWrapper currentWrapper)
        {
            var gameobjectPath = GameObjectExtensions.GetGameObjectTreePath(currentWrapper.gameObject, ModelManager.Instance.CurrentGameObjectRoot.transform);
            foreach (var type in elementTypes)
            {
                MigElement element = (MigElement)Activator.CreateInstance(type);
                element.Wrapper = currentWrapper;
                currentWrapper.PushBackElement(element);
                element.Init(gameobjectPath, Guid.Empty);
                element.Record();
                // the first should be empty
            }
        }
    }

}
