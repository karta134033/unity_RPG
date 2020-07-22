using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Fight;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {

        void Update() {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            print("Out of range");
        }

        public bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
            foreach (RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (Input.GetMouseButtonDown(0)) {
                    print(target);
                    GetComponent<Fighter>().Attack(target);
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
                    GetComponent<Mover>().MoveTo(hit.point);
                    print("InteractWithMovement");
                }
                return true;
            }
            return false;  // 超出地圖邊界
        }
    }
}