using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

//TODO Consider rewiring
using RPG.CameraUI;
namespace RPG.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {



        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;
        [SerializeField] const int stiffLayerNumber = 10;

        //ThirdPersonCharacter playerObject;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster;
        AICharacterControl aiCharControl = null;
        GameObject walkTarget = null;

        //Vector3 currentClickTarget;
        Vector3 clickPoint;
        private Transform m_Cam;

        //bool isInDirectMode = false;


        void Start()
        {

            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }


            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            //playerObject = GetComponent<ThirdPersonCharacter>();
            aiCharControl = GetComponent<AICharacterControl>();
            cameraRaycaster.NotifyLeftMouseClickObservers += ProcessMouseClick;
            walkTarget = new GameObject("walkTarget");
        }



        void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        {
            switch (layerHit)
            {
                case walkableLayerNumber:
                    walkTarget.transform.position = raycastHit.point;
                    aiCharControl.SetTarget(walkTarget.transform);
                    break;
                case enemyLayerNumber:
                    GameObject enemy = raycastHit.collider.gameObject;
                    aiCharControl.SetTarget(enemy.transform);
                    break;
                case stiffLayerNumber:
                    break;
                default:
                    Debug.Log("Don't know how to handle PlayerMovement mouse click");
                    return;

            }
        }


        //private void ProcessDirectMovement()
        //{

        //    float h = CrossPlatformInputManager.GetAxis("Horizontal");
        //    float v = CrossPlatformInputManager.GetAxis("Vertical");

        //    // calculate camera relative direction to move:
        //    Vector3 m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        //    Vector3 m_Move = v * m_CamForward + h * m_Cam.right;

        //    playerObject.Move(v * Vector3.forward + h * Vector3.right, false, false);

        //}






    }
}
