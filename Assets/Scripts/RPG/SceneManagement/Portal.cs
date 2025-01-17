using System;
using System.Collections;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private enum DestinationIdentifier
        {
            A, B, C, D, E, F
        }
        
        [SerializeField] private Transform playerSpawnPoint;
        [Header("Target")]
        [SerializeField] private int targetSceneIndex = -1;
        [SerializeField] private DestinationIdentifier destination = DestinationIdentifier.A;
        [SerializeField] private float fadeOutTime = 1f;
        [SerializeField] private float fadeInTime = 2f;
        [SerializeField] private float fadeWaitTime = .5f;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if (targetSceneIndex < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            var fader = FindFirstObjectByType<Fader>();
            var savingWrapper = FindFirstObjectByType<SavingWrapper>();
            var playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);
            
            UpdatePlayer(this);

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(targetSceneIndex);
            
            var newPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;
            
            savingWrapper.Load();
            
            var targetPortal = GetTargetPortal();
            UpdatePlayer(targetPortal);

            savingWrapper.Save();
            
            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            newPlayerController.enabled = true;
            
            Destroy(gameObject);
        }

        private Portal GetTargetPortal()
        {
            var portals = FindObjectsByType<Portal>(FindObjectsSortMode.None);

            foreach (var portal in portals)
            {
                if (portal == this) continue;
                
                if (portal.destination == destination) return portal;
            }

            Debug.LogWarning($"Target portal with destination {destination} was not found. Player set to default position.");
            return null;
        }

        private void UpdatePlayer(Portal portal)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = portal.playerSpawnPoint.position;
            player.transform.rotation = portal.playerSpawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
        
        private void DisablePlayerControl()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnablePlayerControl()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerController>().enabled = true;
        }

    }
}