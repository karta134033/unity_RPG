using UnityEngine;
using RPG.Core;

namespace RPG.Combat {
    public class Health : MonoBehaviour {
        [SerializeField] float health = 100f;
        bool isDead = false;

        public bool IsDead() {
            return isDead;
        }
        public void TakeDamage(float damage) {
            health = Mathf.Max(health - damage, 0);
            if (health == 0 && !isDead) {
                isDead = true;
                GetComponent<Animator>().SetTrigger("Die");
            }
            print(health);
        }

        private void Die() {
            if (IsDead()) return;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionSchedular>().CancelCurrentAction();
        }
    }
}