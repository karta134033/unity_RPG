using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control {
    public class PlayerController : Agent {

        Health health;
        Fighter fighter;
        public float yawSpeed = 100f;
        new private Rigidbody rigidbody;  // agent的rigidbody
        private float smoothYawChange = 0f;  // 讓動作更加自然

        private float timeCounter = 0f;
    
        private void Start() {
            rigidbody = GetComponent<Rigidbody>();
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
        }

        private void Update() {
            timeCounter += 1;
            if(timeCounter > 2000) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (health.IsDead()) return;
            if (Input.GetKey(KeyCode.Z) && fighter.weaponSweeping()) {
                attackAction();
                return;
            }
            // if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            // print("Mouse location is out of range");
        }

        public void attackAction() {
            fighter.Cancel();  // 取消目前動作
            fighter.TriggerAttack();  // 攻擊目標
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

        public override void OnActionReceived(float[] vectorAction) {
            if (health.IsDead()) return;
            Vector3 move = new Vector3(vectorAction[0], vectorAction[1], vectorAction[2]);
            GetComponent<Mover>().StartMoveAction(transform.position + (move * 2f), 1f);
            Vector3 rotationVector = transform.rotation.eulerAngles;  // 取得現在的rotation
            float yawChange = vectorAction[3];
            smoothYawChange = Mathf.MoveTowards(smoothYawChange, yawChange, 2f * Time.fixedDeltaTime);  // 讓轉換更流暢
            float yaw = rotationVector.y + smoothYawChange * Time.fixedDeltaTime * yawSpeed;
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            if (vectorAction[4] == 1f) attackAction();
        }
        
        public override void CollectObservations(VectorSensor sensor) {
            sensor.AddObservation(transform.localPosition.normalized); 
        }
        public override void Heuristic(float[] actionsOut) {
            Vector3 forword = Vector3.zero;
            Vector3 left = Vector3.zero;
            float yaw = 0f;
            float attack = 0f;
            if (Input.GetKey(KeyCode.W)) forword = transform.forward;

            if (Input.GetKey(KeyCode.A)) left = -transform.right;
            else if (Input.GetKey(KeyCode.D)) left = transform.right;

            if (Input.GetKey(KeyCode.LeftArrow)) yaw = -5f;
            else if (Input.GetKey(KeyCode.RightArrow)) yaw = 5f;

            if (Input.GetKey(KeyCode.Z) && fighter.weaponSweeping()) attack = 1f;

            Vector3 combined = (forword + left).normalized;
            actionsOut[0] = combined.x;
            actionsOut[1] = combined.y;
            actionsOut[2] = combined.z;
            actionsOut[3] = yaw;
            actionsOut[4] = attack;
        }
    }
}