﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Character;


namespace RPG.Weapons
{
    [ExecuteInEditMode]
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickUpSFX;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }
        }

        private void DestroyChildren()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        private void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }

        private void OnTriggerEnter()
        {
            FindObjectOfType<Player>().PickUpWeapon(weaponConfig);
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(pickUpSFX);

        }
    }
}