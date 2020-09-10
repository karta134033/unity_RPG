using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using RPG.Movement;
using RPG.Combat;
using System.Collections.Generic;


namespace RPG.Control {
    public class PlayerController : Agent {

        Health health;
        Fighter fighter;
        new private Rigidbody rigidbody;  // agent的rigidbody
        private float timeCounter = 0f;
        private float currentHealth = 0f;
        private float currentEnemiesHealth = 0f;
        private List<GameObject> enemiesGameObject;

        private void Start() {
            rigidbody = GetComponent<Rigidbody>();
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            currentHealth = health.HealthPoint();
            enemiesGameObject = new List<GameObject>();

            Transform enemies = GameObject.FindWithTag("Enemies").transform;
            for (int i = 0; i < enemies.childCount; i++) {
                Transform child = enemies.GetChild(i);
                if (child.CompareTag("Enemy")) {
                    enemiesGameObject.Add(child.gameObject);
                    currentEnemiesHealth += child.GetComponent<Health>().HealthPoint();
                }
            }
        }

        private void Update() {
            // timeCounter += 1;
            // if (health.IsDead()) return;
            // if (Input.GetKey(KeyCode.Z) && fighter.weaponSweeping()) {
            //     attackAction();
            //     return;
            // }
            // if (InteractWithCombat()) return;
            // if (InteractWithMovement()) return;
            // print("Mouse location is out of range");
            // if(timeCounter > 2000) resetGame();
            float enemiesHealth = 0f;
            foreach (GameObject enemyGameObject in enemiesGameObject) { 
                enemiesHealth += enemyGameObject.GetComponent<Health>().HealthPoint();
            }
            if (health.HealthPoint() < currentHealth) {
                currentHealth = health.HealthPoint();
                AddReward(-.01f);
            }
            if (enemiesHealth < currentEnemiesHealth) {
                currentEnemiesHealth = enemiesHealth;
                AddReward(+.01f);
            }
        }

        public void attackAction() {
            fighter.Cancel();  // 取消目前動作
            fighter.TriggerAttack();  // 攻擊目標
        }

        public void resetGame() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
            foreach (RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (!fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButtonDown(0)) {
                    fighter.Attack(target.gameObject);
                    // print("InteractWithCombat");
                }
                return true;
            }
            return false;
        }
        
        private bool InteractWithMovement() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit) {
                if (Input.GetMouseButtonDown(0)) {
                    print("mouse click: " + hit.point);
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;  // 超出地圖邊界
        }

        public void Cancel() {
            print("PlayerController cancel");
            // GetComponent<Mover>().Cancel();
        }
        public override void OnEpisodeBegin() {
            if (currentEnemiesHealth == 0 || currentHealth == 0) resetGame();
        }
        public override void CollectObservations(VectorSensor sensor) {
            sensor.AddObservation(transform.localPosition.normalized);
            sensor.AddObservation(health.HealthPoint());
        }

        public override void OnActionReceived(float[] vectorAction) {
            if (health.IsDead()) return;
            Vector3 move = new Vector3(vectorAction[0], vectorAction[1], vectorAction[2]);
            GetComponent<Mover>().StartMoveAction(transform.position + (move * 2f), 1f);
            if (vectorAction[3] == 1f) attackAction();
        }
        
        public override void Heuristic(float[] actionsOut) {
            Vector3 forword = Vector3.zero;
            Vector3 left = Vector3.zero;
            float attack = 0f;
            if (Input.GetKey(KeyCode.W)) forword = transform.forward;

            if (Input.GetKey(KeyCode.A)) left = -transform.right;
            else if (Input.GetKey(KeyCode.D)) left = transform.right;

            if (Input.GetKey(KeyCode.Z) && fighter.weaponSweeping()) attack = 1f;

            Vector3 combined = (forword + left).normalized;
            actionsOut[0] = combined.x;
            actionsOut[1] = combined.y;
            actionsOut[2] = combined.z;
            actionsOut[3] = attack;
        }
    }
}