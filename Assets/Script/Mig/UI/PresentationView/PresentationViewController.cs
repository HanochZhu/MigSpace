using Mig.Model;
using Mig.Snapshot;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mig.UI.Presentation
{
    public class PresentationViewController : MonoBehaviour
    {
        public Button ExitPresentation;
        public Button Next;
        public Button Previous;

        public Text PagesText;

        public Action OnExitPresentationCallback;

        private void Awake()
        {
            PagesText.text = string.Format("{0} of {1}", SnapshotManager.Instance.CurrentSnapshotIndex, SnapshotManager.Instance.CurrentSnapshotCount);
        }

        private void OnEnable()
        {
            ExitPresentation.onClick.AddListener(OnExitPresentation);
            Next.onClick.AddListener(() => {
                SnapshotManager.Instance.ApplyToTargetSnapshot(SnapshotManager.Instance.CurrentSnapshotIndex + 1);
                PagesText.text = string.Format("{0} of {1}", SnapshotManager.Instance.CurrentSnapshotIndex, SnapshotManager.Instance.CurrentSnapshotCount);
            });
            Previous.onClick.AddListener(() =>
            {
                SnapshotManager.Instance.ApplyToTargetSnapshot(SnapshotManager.Instance.CurrentSnapshotIndex - 1);
                PagesText.text = string.Format("{0} of {1}", SnapshotManager.Instance.CurrentSnapshotIndex, SnapshotManager.Instance.CurrentSnapshotCount);
            });
        }

        private void OnDisable()
        {
            ExitPresentation.onClick.RemoveAllListeners();
            Next.onClick.RemoveAllListeners();
            Previous.onClick.RemoveAllListeners();  
        }

        private void OnExitPresentation()
        {
            OnExitPresentationCallback?.Invoke();
        }
    }
}
