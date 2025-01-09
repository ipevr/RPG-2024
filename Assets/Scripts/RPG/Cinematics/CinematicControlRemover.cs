using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using RPG.Control;
using RPG.Core;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        [SerializeField] private InputAction cancelCinematic;
        
        private GameObject player;
        
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        private void Update()
        {
            if (cancelCinematic.IsPressed())
            {
                GetComponent<PlayableDirector>().Stop();
            }
        }

        private void DisableControl(PlayableDirector playableDirector)
        {
            cancelCinematic.Enable();
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            cancelCinematic.Disable();
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
