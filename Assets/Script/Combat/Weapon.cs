using UnityEngine;
using RPG.Core;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 50f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHand = true;
        [SerializeField] Projectile projectile = null;


        public void Spawn(Transform rightHand, Transform leftHand, Animator animator) {
            if (equippedPrefab != null) {
                Transform handTransform = GetTransform(rightHand, leftHand);
                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOverride != null) {
                animator.runtimeAnimatorController = animatorOverride;            
            }
        }

        public Transform GetTransform(Transform rightHand, Transform leftHand) {
            Transform handTransform;
            if (isRightHand) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile() {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target) {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }

        public float GetDamage() {
            return weaponDamage;
        }

        public float GetRange() {
            return weaponRange;
        }
    }
}