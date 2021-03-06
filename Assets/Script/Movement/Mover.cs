﻿using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, ActionInterface, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 5f;
        NavMeshAgent navMeshAgent;
        Health health;
        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update() {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimatior();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction) {
            GetComponent<ActionSchedular>().SetAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction) {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel() {
            if(navMeshAgent.isActiveAndEnabled) {  // 避免fighter調用此方法時出問題
                navMeshAgent.isStopped = true;
            }
        }

        private void UpdateAnimatior() {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

        public object CaptureState() {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state) {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;  // 為了讓NavMeshAgent可以更新位置
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}