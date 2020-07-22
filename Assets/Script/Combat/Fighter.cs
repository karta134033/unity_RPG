using UnityEngine;
using RPG.Combat;
using RPG.Movement;

namespace RPG.Fight {
    public class Fighter : MonoBehaviour {

        [SerializeField] float weaponRange = 5f;
        Transform target;
        private void Update() {
            if (target != null) {
                bool isInRange = Vector3.Distance(transform.position, target.position) < weaponRange;
                if (!isInRange) {
                    GetComponent<Mover>().MoveTo(target.position);
                }
                else {
                    GetComponent<Mover>().Stop();
                    target = null;
                }
            }
        }
        public void Attack(CombatTarget combatTarget) {
            print("Take that");
            target = combatTarget.transform;
        }
    }
}