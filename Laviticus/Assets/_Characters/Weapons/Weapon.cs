using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Character
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject
    {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnim;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;

        public float GetMinTimeBetweenHits()
        {

            return minTimeBetweenHits;

        }

        public float GetMaxAttackRange()
        {

            return maxAttackRange;

        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimClip()
        {
            StripAnimEvents();
            return attackAnim;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        //Prevents asset packs creating bugs
        private void StripAnimEvents()
        {
            attackAnim.events = new AnimationEvent[0];
        }
    }
}