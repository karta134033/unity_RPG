using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinematicTrigger : MonoBehaviour {

        bool triggerCheck = false;
        private void OnTriggerEnter(Collider other) {
            if (triggerCheck || other.gameObject.tag != "Player") return;
            GetComponent<PlayableDirector>().Play();
            triggerCheck = true;
        }
    }
}
    
