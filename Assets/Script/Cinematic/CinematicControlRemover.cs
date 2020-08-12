using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics {
    public class CinematicControlRemover : MonoBehaviour {
        GameObject player;
        void Start() {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }

        void DisableControl(PlayableDirector pd) {
            print ("DisableControl");
            player.GetComponent<ActionSchedular>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd) {
            print ("EnableControl");
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
