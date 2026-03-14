using Mig.Core;
using Mig.UI;

namespace Mig
{

    public class MigManager : JigSingleton<MigManager>
    {
        public UIController UIController;

        private MigEditorController editorController;

        private MigPresentationController presentationController;

        private MigLoadingProjectController loadingController;

        private StateMachine stateMachine;

        private new void Awake()
        {
            stateMachine = new StateMachine();

            editorController = new(UIController, onEnterPresentation);
            presentationController = new(UIController, onExitPresentation);
            loadingController = new(UIController, onLoadingComplete);


            stateMachine.ChangeState(loadingController);
        }

        private void onLoadingComplete(bool isSuccess)
        {
            stateMachine.ChangeState(editorController);
        }

        private void onEnterPresentation()
        {
            stateMachine.ChangeState(presentationController);
        }

        private void onExitPresentation()
        {
            stateMachine.ChangeState(editorController);
        }

        private void Update()
        {
            stateMachine.Update();
        }

        private void OnDisable()
        {
            stateMachine.Stop();
        }
    }

}
