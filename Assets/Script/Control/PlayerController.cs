using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Fight;
using RPG.Core;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour, ActionInterface {

        void Update() {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            print("Mouse location is out of range");
        }

        public bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
            foreach (RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (!GetComponent<Fighter>().CanAttack(target)) continue;

                if (Input.GetMouseButtonDown(0)) {
                    GetComponent<Fighter>().Attack(target);
                    print("InteractWithCombat");
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit) {
                if (Input.GetMouseButtonDown(0)) {
                    GetComponent<ActionSchedular>().SetAction(this);
                    GetComponent<Mover>().MoveTo(hit.point);
                    // print("InteractWithMovement");
                }
                return true;
            }
            return false;  // 超出地圖邊界
        }

        public void Cancel() {
            print("PlayerController cancel");
        }
    }
}