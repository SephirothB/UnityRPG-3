using UnityEngine;

namespace RPG.Character
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[SerializeField] float movingTurnSpeed = 360;
		[SerializeField] float stationaryTurnSpeed = 180;
		[SerializeField] float runCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float moveSpeedMultiplier = 1f;
		
		Rigidbody myRigidbody;
		Animator characterAnimator;
		float turnAmount;
		float forwardAmount;
		Vector3 m_GroundNormal;
		
		void Start()
		{
			characterAnimator = GetComponent<Animator>();
			myRigidbody = GetComponent<Rigidbody>();
		
			myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			
			characterAnimator.applyRootMotion = true;

		}


		public void Move(Vector3 move)
		{

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			turnAmount = Mathf.Atan2(move.x, move.z);
			forwardAmount = move.z;

			ApplyExtraTurnRotation();


			// send input and other state parameters to the animator
			UpdateAnimator(move);
		}


		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			characterAnimator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
			characterAnimator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					characterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);

		}



		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
			transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
		}


		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (Time.deltaTime > 0)
			{
				Vector3 v = (characterAnimator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = myRigidbody.velocity.y;
				myRigidbody.velocity = v;
			}
		}


		
	}
}
