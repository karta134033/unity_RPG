using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, ActionInterface {

        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        float timeSinceLastAttack = Mathf.Infinity;
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

        public bool weaponSweeping() {  // 確認武器可以揮動
            return !currentWeapon.HasProjectile();
        }
        public void EquipWeapon(Weapon weapon) {
            if (weapon == null) return;
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
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

        public void StopAttack() {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }

        public void TriggerAttack() {  // 避免沒有reset而出現小bug
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        void Hit() {  // 來自於動畫的事件
            if (target == null) return;
            if (currentWeapon.HasProjectile()) {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            } else {
                target.TakeDamage(currentWeapon.GetDamage());
            }
        }

        void Shoot() {  // 來自於動畫弓箭射擊的事件
            Hit();
        }
    }
}