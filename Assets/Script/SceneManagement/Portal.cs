using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {

        [SerializeField] int sceneToLoad = -1;
        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                SceneManager.LoadScene(sceneToLoad);
                print("sceneToLoad: " + sceneToLoad);
            }
        }

        private IEnumerator Transition() {
            yield return SceneManager.LoadSceneAsync(sceneToLoad);;
            print("Scene Loaded");
        }
    }
}