using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        Health  target = null;
        float damage = 0;

        // Update is called once per frame
        void Update() {
            if (target == null) return;
            transform.LookAt(GetAimLoaction());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage) {
            this.target = target;
            this.damage = damage;
        }

        private Vector3 GetAimLoaction() {  // 取得目標大約身體一半的位置
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return target.transform.position;  // 如果沒有collider直接回傳位置
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != target) return;
            target.TakeDamage(damage);
            Destroy(gameObject);  // 刪除箭的物件
        }
    }
}
