
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

//TODO Consider rewiring
using RPG.CameraUI;
using RPG.Weapons;
using RPG.Core;
using System;
using System.Collections;

namespace RPG.Character
{
    [RequireComponent(typeof(AudioSource))]
    public class Player : MonoBehaviour, IDamagable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;

        [SerializeField] Weapon weaponInUse;
        [SerializeField] AnimatorOverrideController animOverrideController;

        [SerializeField] SpecialAbility[] abilities;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        const string DEATH_ANIM = "Death";
        const string ATTACK_ANIM = "Attack";

        AudioSource audio;
        Animator animator;
        PowerAttackConfig config;

        private float currentHealthPoints = 100f;
        float lastHitTime = 0f;
        GameObject currentTarget;
        CameraRaycaster cameraRaycaster;

        //bool playerisDead;
        //public bool PlayerIsDead
        //{
        //    get
        //    {
        //        return playerisDead;
        //    }
        //}
        public float HealthAsPercentage
        {
            get
            {
                return currentHealthPoints / (float)maxHealthPoints;
            }
        }



        public void TakeDamage(float damage)
        {
            bool playerIsDead = (currentHealthPoints - damage <= 0);
            ReduceHealth(damage);
            
            if (playerIsDead)
            {

                              
                StartCoroutine(KillPlayer());
                //Play death sound

                //Trigger death animation

                //Reload the scene

                //TODO remove to allow death and implement reload funtionality //if (currentHealthPoints <= 0) { Destroy(gameObject); }
            }
            
        }

        IEnumerator KillPlayer()
        {
            animator.SetTrigger(DEATH_ANIM);

            audio.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            //TODO remove once ready audio.Play();
            
            yield return new WaitForSecondsRealtime(audio.clip.length);

            SceneManager.LoadScene(0);
            
        }

        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            audio.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            //TODO remove once ready audio.Play();
        }


        // Use this for initialization
        void Start()
        {


            RegisterForMouseClick();
            SetCurrentMaxHealth();
            RegisterWeaponInUse();
            SetupRuntimeAnimator();
            abilities[0].AttachComponent(gameObject);
            audio = GetComponent<AudioSource>();


        }
        void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            //currentTarget = enemy;
            //var enemyHit = enemy.gameObject;

            if (Input.GetMouseButtonDown(0) && IsEnemyInRange(enemy))
            {
                AttackTarget(enemy);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                AttemptSpecialAbility(0, enemy);

            }
        }

        private void AttemptSpecialAbility(int abilityIndex, Enemy enemy)
        {
            
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energyComponent.IsEnergyAvailable(energyCost))
            {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Engage(abilityParams);
            }
        }

        void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        void RegisterWeaponInUse()
        {

            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject weaponSocket = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, weaponSocket.transform); //TODO Move to hand

            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;

        }

        void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animOverrideController;
            animOverrideController["Default attack"] = weaponInUse.GetAttackAnimClip(); //Remove Constant
        }

       

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();

            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No Dominant Hand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "More than on Dominant Hand scripts on player please remove one");
            return dominantHands[0].gameObject;

        }

        


        private bool IsEnemyInRange(Enemy target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();    
        }


        private void AttackTarget(Enemy target)
        {

           if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger(ATTACK_ANIM);
                target.TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }

        }

        //Energy code below
       
        // Update is called once per frame
        void Update()
        {

        }
    }
}