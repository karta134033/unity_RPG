using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, ActionInterface {

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] float attackDamage = 20f;
        [SerializeField] float timeSinceLastAttack = Mathf.Infinity;
        Health target;

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;

            if (target != null) {
                bool isInRange = Vector3.Distance(transform.position, target.transform.position) < weaponRange;
                if (target.IsDead()) return;
                if (!isInRange) {
                    GetComponent<Mover>().MoveTo(target.transform.position, 1f);
                }
                if (isInRange) {
                    GetComponent<Mover>().Cancel();
                    AttackBehavior();
                }
            }
        }

        private void AttackBehavior() {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timeBetweenAttack) {
                // 以下會觸發void Hit()
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        public bool CanAttack(GameObject combatTarget) {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget) {
            // print("Attack");
            GetComponent<ActionSchedular>().SetAction(this);
            target = combatTarget.GetComponent<Health>();  // 取得攻擊目標
        }

        public void Cancel() {
            // print("Fighter cancel");
            target = null;
            StopAttack();
        }

        private void StopAttack() {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }

        private void TriggerAttack() {  // 避免沒有reset而出現小bug
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        void Hit() {  // 來自於動畫的事件
            if (target != null) target.TakeDamage(attackDamage);
        }
    }
}