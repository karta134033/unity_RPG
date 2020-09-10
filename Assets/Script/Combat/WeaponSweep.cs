using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponSweep : MonoBehaviour {
        [SerializeField] Weapon weapon = null;
        bool attack = false;

        void Update() {
            if (Input.GetKey(KeyCode.Z)) attack = true; // 有按下A才會觸發傷害
        }
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Enemy" && attack) {
                other.GetComponent<Health>().TakeDamage(weapon.GetDamage());
                attack = false;  // 重置
            }
        }
    }

}
