using UnityEngine;
using RPG.Core;

namespace RPG.Combat {
    [RequireComponent(typeof(Health))]  // 避免誤刪Health
    public class CombatTarget : MonoBehaviour {

    }
}