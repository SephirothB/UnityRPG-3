using System;
using UnityEngine;
using UnityEngine.AI;

//TODO Consider rewiring
using RPG.CameraUI;
namespace RPG.Character
{
    [RequireComponent(typeof(NavMeshAgent))]

    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {

        CameraRaycaster cameraRaycaster;

        GameObject walkTarget = null;

        //Vector3 currentClickTarget;
        Vector3 clickPoint;
        private Transform m_Cam;

        //bool isInDirectMode = false;


        void Start()
        {

            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();

            cameraRaycaster.SendDestinationVector += SetDestinationVector;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            walkTarget = new GameObject("walkTarget");
        }

        void SetDestinationVector(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                //aiCharControl.SetTarget(walkTarget.transform);
            }
        }


        void OnMouseOverEnemy(Enemy enemy)
        {

            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                //aiCharControl.SetTarget(enemy.transform);
            }

        }

    }
}
