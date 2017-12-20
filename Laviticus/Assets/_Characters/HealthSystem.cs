using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG.Character
{
    public class HealthSystem : MonoBehaviour
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthBar;
        [SerializeField] float deathVanishSeconds = 2.0f;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        const string DEATH_ANIM = "Death";

        [SerializeField] float currentHealthPoints;

        AudioSource audioPlayer;
        Animator animator;
        CharacterMovement characterMovement;


        public float HealthAsPercentage
        {
            get
            {
                return currentHealthPoints / (float)maxHealthPoints;
            }
        }
        // Use this for initialization
        void Start()
        {
            currentHealthPoints = maxHealthPoints;
            animator = GetComponent<Animator>();
            audioPlayer = GetComponent<AudioSource>();
            characterMovement = GetComponent<CharacterMovement>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (healthBar)
            {
                healthBar.fillAmount = HealthAsPercentage;
            }
        }

        public void TakeDamage(float damage)
        {
            bool characterDies = (currentHealthPoints - damage <= 0);
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            var damageClip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioPlayer.PlayOneShot(damageClip);

            if (currentHealthPoints <= 0)
            {


                StartCoroutine(KillCharacter());


                //TODO remove to allow death and implement reload funtionality //if (currentHealthPoints <= 0) { Destroy(gameObject); }
            }

        }

        public void AddHealth(float healAmount)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + healAmount, 0f, maxHealthPoints);
        }

        IEnumerator KillCharacter()
        {
            StopAllCoroutines();
            characterMovement.Kill();

            animator.SetTrigger(DEATH_ANIM);

            var playerComponent = GetComponent<Player>();
            if (playerComponent && playerComponent.isActiveAndEnabled) //research lazy evaluation
            {
                audioPlayer.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
                audioPlayer.Play();

                yield return new WaitForSecondsRealtime(audioPlayer.clip.length);

                SceneManager.LoadScene(0);
            }
            else
            {
                Destroy(gameObject, deathVanishSeconds);
            }



        }
    }
}