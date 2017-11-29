using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

using RPG.Weapons;
using RPG.Core;

namespace RPG.Character
{
    public class Enemy : MonoBehaviour, IDamagable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagePerShot = 1f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float secondsBetweenAttack = 1f;
        [SerializeField] float moveToAttackRadius = 6f;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;

        [SerializeField] Vector3 verticalAimOffset = new Vector3(0, 1f, 0);
        private float currentHealthPoints;

        AICharacterControl aiCharacterControl = null;
        GameObject player = null;
        GameObject fireFrom;

        bool isAttacking = false;


        public float HealthAsPercentage
        {
            get
            {
                return currentHealthPoints / (float)maxHealthPoints;
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }

        // Use this for initialization
        void Start()
        {

            player = GameObject.FindGameObjectWithTag("Player");
            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
            //fireFrom = FindGameObjectWithTag("ProjectileSpawnPoint");


        }

        // Update is called once per frame
        void Update()
        {

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= moveToAttackRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;

                InvokeRepeating("FireProjectile", 0f, secondsBetweenAttack); //TODO switch to coroutines


            }
            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }


        }

        void FireProjectile()
        {


            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetFirer(gameObject);

            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            Vector3 unitVectorToPlayer = (player.transform.position + verticalAimOffset - projectileSocket.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;

            //Destroy(newProjectile, 0.8f);

        }

        //void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(transform.position, attackRadius);

        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(transform.position, moveToAttackRadius);
        //}
    }
}