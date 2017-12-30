using System;
using System.Collections;
using UnityEngine;

namespace RPG.Character
{
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;

        const string ATACK_ANIM = "Attack";

        float currentWeaponRange;
        bool isAttacking = false;
        float distanceToPlayer;
        int nextWaypointIndex;
        float waypointTolerance = 2f;

        AudioSource audioPlayer;
        Animator animator;
        PlayerControl player;
        Character character;

        enum State { idle, patrolling, attacking, chasing }
        State state = State.idle;



        void Start()
        {
            player = FindObjectOfType<PlayerControl>();
            character = GetComponent<Character>();
            audioPlayer = GetComponent<AudioSource>();
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                StartCoroutine("Patrolling");
            }

            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                StopAllCoroutines();
                StartCoroutine("ChasePlayer");
            }

            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                StopAllCoroutines();
                state = State.attacking;
            }
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator Patrolling()
        {
            state = State.patrolling;
            while (distanceToPlayer > chaseRadius)
            {
                Vector3 nextWaypointPosition = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPosition);
                CycleWaypoint(nextWaypointPosition);
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void CycleWaypoint(Vector3 nextWaypointPosition)
        {
            if (Vector3.Distance(transform.position, nextWaypointPosition) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
        }
    }
}

