using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Character
{
    public class TeslaAttackBehaviour : MonoBehaviour, SpecialAbility.ISpecialAbility
    {
        TeslaAttackConfig config;

        int layerMask = 1 << 8;

        
        public void SetConfig(TeslaAttackConfig configToSet)
        {
            this.config = configToSet;
        }

        public void Engage(AbilityUseParams useParams)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position, 
                config.GetRadius(), 
                Vector3.up, 
                config.GetRadius()
                );
            

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamagable>();

                if (damageable != null)
                {
                    float damagetoDeal = useParams.baseDamage + config.GetDamageMulti();
                    damageable.TakeDamage(damagetoDeal);
                }
            }
           
        }

    }
}
