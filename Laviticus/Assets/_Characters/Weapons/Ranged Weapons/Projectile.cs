using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;


namespace RPG.Character
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] const int enemyLayerNumber = 9;
        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject projectileFirer;

        const float destroyDelay = 0.01f;
        float damageCaused;

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public void SetFirer(GameObject projectileFirer)
        {
            this.projectileFirer = projectileFirer;
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var layerCollidedWith = collision.gameObject.layer;
            if (projectileFirer && layerCollidedWith != projectileFirer.layer)
            {
                //DealDamage(collision);

            }



        }

        //private void DealDamage(Collision collision)
        //{
        //    Component damagableComponent = collision.gameObject.GetComponent(typeof(IDamagable));
        //    if (damagableComponent)
        //    {
        //        (damagableComponent as IDamagable).TakeDamage(damageCaused);

        //    }

        //    Destroy(gameObject, destroyDelay);
        //}
    }
}