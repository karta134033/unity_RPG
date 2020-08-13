using System;
using UnityEngine.AI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() {
            if (sceneToLoad < 0) {
                Debug.LogError("Scene to load");
                yield break;
            }
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(2f);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(0.5f);
            yield return fader.FadeIn(1f);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal) {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            // print("otherPortal.spawnPoint.rotation;" + otherPortal.spawnPoint.rotation);
            // player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal() {
            foreach(Portal portal in FindObjectsOfType<Portal>()) {
                if (portal == this) continue;
                return portal;
            }
            return null;
        }
    }
}