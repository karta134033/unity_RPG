using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Fight {
    public class Fighter : MonoBehaviour, ActionInterface {

        [SerializeField] float weaponRange = 5f;
        Transform target;

        private void Update() {
            if (target != null) {
                bool isInRange = Vector3.Distance(transform.position, target.position) < weaponRange;
                if (!isInRange) {
                    GetComponent<Mover>().MoveTo(target.position);
                }
                if (isInRange) {
                    GetComponent<Mover>().Cancel();
                    target = null;
                }
            }
        }

        public void Attack(CombatTarget combatTarget) {
            print("Attack");
            GetComponent<ActionSchedular>().SetAction(this);
            target = combatTarget.transform;
        }

        public void Cancel() {
            print("Fighter cancel");
            target = null;
        }
    }
}