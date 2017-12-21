using System;
using UnityEngine;
using UnityEngine.AI;

//TODO Consider rewiring
using RPG.CameraUI;
namespace RPG.Character
{
    [SelectionBase]
   
    public class Character : MonoBehaviour
    {
        [Header("Rigid Body Settings")]
        [SerializeField] float rbMass;
        [SerializeField] float rbDrag;
        [SerializeField] float rbAngularDrag;
        [SerializeField] bool useGravity;

        [Header("Capsule Collider Settings")]
        [SerializeField] Vector3 capsuleCenter = new Vector3(0, 0.86f, 0);
        [SerializeField] float capsuleRadius;
        [SerializeField] float capsuleHeight;

        [Header("Animator Settings")]
        [SerializeField] RuntimeAnimatorController animController;
        [SerializeField] AnimatorOverrideController animOverride;
        [SerializeField] Avatar characterAvatar;

        [Header("Movement Properties")]
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float movementSpeedMultiplier = 0.7f;
        [SerializeField] float moveThreshold = 1f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;

        [Header("NavMesh Settings")]
        [SerializeField] float nmSpeed = 3.5f;
        [SerializeField] float nmAngularSpeed = 120f;
        [SerializeField] float nmAcceleration = 8f;
        [SerializeField] float nmStoppingDistance = 1.3f;
        [SerializeField] bool nmAutoBraking = false;

        [Header("Audio Source")]
        [SerializeField]
        float audioSourceSpatialBlend = 0.5f;


        Rigidbody myRigidbody;
        NavMeshAgent navAgent;
        Animator characterAnimator;

        float turnAmount;
        float forwardAmount;
        bool isAlive = true;

        private void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.mass = rbMass;
            myRigidbody.drag = rbDrag;
            myRigidbody.angularDrag = rbAngularDrag;
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = capsuleCenter;
            capsuleCollider.radius = capsuleRadius;
            capsuleCollider.height = capsuleHeight;

            characterAnimator = gameObject.AddComponent<Animator>();
            characterAnimator.runtimeAnimatorController = animController;
            characterAnimator.avatar = characterAvatar;

            navAgent = gameObject.AddComponent<NavMeshAgent>();
            navAgent.speed = nmSpeed;
            navAgent.angularSpeed = nmAngularSpeed;
            navAgent.acceleration = nmAcceleration;
            navAgent.stoppingDistance = nmStoppingDistance;
            navAgent.autoBraking = nmAutoBraking;
            navAgent.updateRotation = false;
            navAgent.updatePosition = true;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;
        }

        void Start()
        {
       
            characterAnimator.applyRootMotion = true;

        }

        private void Update()
        {
            if (navAgent.remainingDistance > navAgent.stoppingDistance && isAlive)
            {
                Move(navAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
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

        public void SetDestination(Vector3 worldPos)
        {
            navAgent.destination = worldPos;
        }
        void Move(Vector3 movement)
        {

            SetForwardAndTurn(movement);

            ApplyExtraTurnRotation();

            UpdateAnimator();
        }

        public void Kill()
        {
            isAlive = false;
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
