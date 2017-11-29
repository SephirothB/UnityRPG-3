using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//TODO Consider rewiring
using RPG.CameraUI;
using RPG.Weapons;
using RPG.Core;

namespace RPG.Character
{
    public class Player : MonoBehaviour, IDamagable
    {

        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;
        [SerializeField] const int stiffLayerNumber = 10;

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damageCaused = 10f;

        [SerializeField] Weapon weaponInUse;
        [SerializeField] AnimatorOverrideController animOverrideController;

        Animator animator;

        private float currentHealthPoints = 100f;
        float lastHitTime = 0f;

        GameObject currentTarget;
        CameraRaycaster cameraRaycaster;


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
            //if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }


        // Use this for initialization
        void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            RegisterWeaponInUse();
            SetupRuntimeAnimator();


        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animOverrideController;
            animOverrideController["Default attack"] = weaponInUse.GetAttackAnimClip(); //Remove Constant
        }

        private void RegisterWeaponInUse()
        {

            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject weaponSocket = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, weaponSocket.transform); //TODO Move to hand

            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;

        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();

            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No Dominant Hand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "More than on Dominant Hand scripts on player please remove one");
            return dominantHands[0].gameObject;

        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.NotifyLeftMouseClickObservers += OnLeftMouseClick;
        }

        void OnLeftMouseClick(RaycastHit raycastHit, int layerHit)
        {

            if (layerHit == enemyLayerNumber)
            {

                var enemy = raycastHit.collider.gameObject;
                currentTarget = enemy;

                if (IsEnemyInRange(enemy))
                {
                    AttackTarget(enemy);
                }

                             
                
            }

        }

        private bool IsEnemyInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();    
        }


        private void AttackTarget(GameObject target)
        {

            //Component damagableComponent = target.gameObject.GetComponent(typeof(IDamagable));
            var enemyComponent = target.GetComponent<Enemy>();

            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack");
                enemyComponent.TakeDamage(damageCaused);
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