using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, ActionInterface {

        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Transform handTransform = null;
        Health target;
        Weapon currentWeapon = null;

        private void Start() {
            EquipWeapon(defaultWeapon);
        }
        
        private void Update() {
            timeSinceLastAttack += Time.deltaTime;

            if (target != null) {
                bool isInRange = Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
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

        public void EquipWeapon(Weapon weapon) {
            if (weapon == null) return;
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(handTransform, animator);
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
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
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
            if (target != null) target.TakeDamage(currentWeapon.GetDamage());
        }
    }
}