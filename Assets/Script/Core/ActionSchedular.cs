using UnityEngine;

namespace RPG.Core {
    public class ActionSchedular : MonoBehaviour {
        ActionInterface currentAction;

        public void SetAction(ActionInterface action) {
            if (action == currentAction) return;
            if (action != null) {
                if (currentAction != null) currentAction.Cancel();
                print ("Change Action from: " + currentAction + "to :" + action);
                currentAction = action;
            }
        }

        public void CancelCurrentAction() {
            SetAction(null);
        }
    }
}    
