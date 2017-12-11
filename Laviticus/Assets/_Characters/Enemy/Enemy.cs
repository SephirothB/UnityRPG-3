using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Weapons;
using RPG.Core;

namespace RPG.Character
{
    public class Enemy : MonoBehaviour, IDamagable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagePerShot = 1f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float firingPeriodSeconds = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] float moveToAttackRadius = 6f;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] AnimatorOverrideController animOverrideController;
        [SerializeField] Vector3 verticalAimOffset = new Vector3(0, 1f, 0);
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        private float currentHealthPoints;

        

        AudioSource audioPlayer;
        Animator animator;
        AICharacterControl aiCharacterControl = null;
        Player player = null;
        GameObject fireFrom;

        const string DEATH_ANIM = "Death";
        const string ATACK_ANIM = "Attack";
        
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
            bool enemyIsDead = (currentHealthPoints - damage <= 0);
            ReduceHealth(damage);
            if (enemyIsDead)
            {


                StartCoroutine(KillEnemy());
                //Play death sound

                //Trigger death animation

                //Reload the scene

                //TODO remove to allow death and implement reload funtionality //if (currentHealthPoints <= 0) { Destroy(gameObject); }
            }

        }

        IEnumerator KillEnemy()
        {
            print("Enemy Dead");
            
            audioPlayer.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioPlayer.Play();
            Destroy(gameObject);

            //animator.SetTrigger(DEATH_ANIM);

            yield return new WaitForSecondsRealtime(audioPlayer.clip.length);

            
            
        }
        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            audioPlayer.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            //TODO remove once ready audio.Play();
        }
        // Use this for initialization
        void Start()
        {

            player = FindObjectOfType<Player>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
            audioPlayer = GetComponent<AudioSource>();
            //fireFrom = FindGameObjectWithTag("ProjectileSpawnPoint");


        }

        // Update is called once per frame
        void Update()
        {

            if (player.HealthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this);
            }
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
                float randomisedDelay = Random.Range(firingPeriodSeconds - firingPeriodVariation, firingPeriodSeconds + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay); //TODO switch to coroutines

               
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