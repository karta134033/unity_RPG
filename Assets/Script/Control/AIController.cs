using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Control {
    public class AIController : MonoBehaviour {
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] float chaseDistance = 5f;
        float suspicionTime = 10f;
        float patrolSpeedFractiohn = 0.4f;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArriveWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
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
                timeSinceLastSawPlayer = 0;
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime) {
                SuspicionBehavior();
            }
            else {
                PatrolBehavior();
            }
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArriveWaypoint += Time.deltaTime;
        }


        private bool AtWaypoint() {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        private void CycleWaypoint() {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint() {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehavior() {
            fighter.Cancel();
        }

        private void PatrolBehavior() {
            fighter.Cancel();
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null) {
                if (AtWaypoint()) {
                    timeSinceArriveWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArriveWaypoint > waypointDwellTime) {
                mover.StartMoveAction(nextPosition, patrolSpeedFractiohn);
            }
        }
        private void AttackBehavior() {
            fighter.Attack(player);
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