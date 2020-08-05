using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;

namespace RPG.Control {
    public class AIController : MonoBehaviour {
        float chaseDistance = 5f;
        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;
        Vector3 guardPosition;
        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = transform.position;
            player = GameObject.FindWithTag("Player");
        }

        private void Update() {
            if (health.IsDead()) {
                fighter.Cancel();
                return;
            }
            if (InAttackRange() && fighter.CanAttack(player)) {
                fighter.Attack(player);
            } 
            else {
                fighter.Cancel();
                mover.MoveTo(guardPosition);
            }
        }

        private bool InAttackRange() {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}