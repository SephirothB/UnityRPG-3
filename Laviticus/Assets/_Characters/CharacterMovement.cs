using System;
using UnityEngine;
using UnityEngine.AI;

//TODO Consider rewiring
using RPG.CameraUI;
namespace RPG.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float movementSpeedMultiplier = 0.7f;
        [SerializeField] float moveThreshold = 1f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;


        Rigidbody myRigidbody;
        NavMeshAgent navAgent;
        Animator characterAnimator;

        float turnAmount;
        float forwardAmount;

         void Start()
        {

            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            
            characterAnimator = GetComponent<Animator>();

            myRigidbody = GetComponent<Rigidbody>();
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            characterAnimator.applyRootMotion = true;

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
                Move(navAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
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

        void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (characterAnimator.deltaPosition * movementSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = myRigidbody.velocity.y;
                myRigidbody.velocity = velocity;
            }
        }

        public void Move(Vector3 movement)
        {

            SetForwardAndTurn(movement);

            ApplyExtraTurnRotation();

            UpdateAnimator();
        }

        public void Kill()
        {
            //Stop everything because character has died
        }

        void SetForwardAndTurn(Vector3 movement)
        {
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }

            var localMovement = transform.InverseTransformDirection(movement);

            turnAmount = Mathf.Atan2(localMovement.x, localMovement.z);
            forwardAmount = localMovement.z;
        }

        void UpdateAnimator()
        {
            // update the animator parameters
            characterAnimator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            characterAnimator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            //characterAnimator.speed = movementSpeedMultiplier;

        }



        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }
    }
}
