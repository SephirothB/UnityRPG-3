using System;
using UnityEngine;
using UnityEngine.AI;

//TODO Consider rewiring
using RPG.CameraUI;
namespace RPG.Character
{
    [RequireComponent(typeof(NavMeshAgent))]

    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stoppingDistance = 1f;

        ThirdPersonCharacter character;
        GameObject walkTarget;
        NavMeshAgent navAgent;
        //Vector3 currentClickTarget;
        Vector3 clickPoint;
    

        void Start()
        {

            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            character = GetComponent<ThirdPersonCharacter>();
            walkTarget = new GameObject("walkTarget");

            navAgent = GetComponent<NavMeshAgent>();
            navAgent.updateRotation = false;
            navAgent.updatePosition = true;
            navAgent.stoppingDistance = stoppingDistance;

            cameraRaycaster.SendDestinationVector += SetDestinationVector;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        private void Update()
        {
            if (navAgent.remainingDistance > navAgent.stoppingDistance)
            {
                character.Move(navAgent.desiredVelocity);
            }
            else
            {
                character.Move(Vector3.zero);
            }
        }

        void SetDestinationVector(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {

                navAgent.SetDestination(destination);

            }
        }


        void OnMouseOverEnemy(Enemy enemy)
        {

            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                navAgent.SetDestination(enemy.transform.position);
            }

        }

    }
}
