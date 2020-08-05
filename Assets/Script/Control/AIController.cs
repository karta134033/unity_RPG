using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Control {
    public class AIController : MonoBehaviour {
        float chaseDistance = 5f;
        Fighter fighter;
        Health health;
        GameObject player;
        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
        }

        private void Update() {
            if (health.IsDead()) {
                fighter.Cancel();
                return;
            }
            if (InAttackRange() && fighter.CanAttack(player)) {
                GetComponent<Fighter>().Attack(player);
            } 
            else {
                fighter.Cancel();
            }
        }

        private bool InAttackRange() {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }
    }
}