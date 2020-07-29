using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Fight {
    public class Fighter : MonoBehaviour, ActionInterface {

        [SerializeField] float weaponRange = 5f;
        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] float attackDamage = 10f;
        Transform target;
        float timeSinceLastAttack = 0;

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;

            if (target != null) {
                bool isInRange = Vector3.Distance(transform.position, target.position) < weaponRange;
                if (!isInRange) {
                    GetComponent<Mover>().MoveTo(target.position);
                }
                if (isInRange) {
                    GetComponent<Mover>().Cancel();
                    AttackBehavior();
                }
            }
        }

        private void AttackBehavior() {
            if(timeSinceLastAttack > timeBetweenAttack) {
                // 以下會觸發void Hit()
                GetComponent<Animator>().SetTrigger("Attack");
                timeSinceLastAttack = 0;
            }
        }

        public void Attack(CombatTarget combatTarget) {
            print("Attack");
            GetComponent<ActionSchedular>().SetAction(this);
            target = combatTarget.transform;  // 取得攻擊目標
            print("target: " + target);
        }

        public void Cancel() {
            print("Fighter cancel");
            target = null;
        }

        void Hit() {  // 來自於動畫的事件
            if (target != null) target.GetComponent<Health>().TakeDamage(attackDamage);
        }
    }
}