using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playground.MVC
{
    public class Damager : MonoBehaviour
    {
        [SerializeField] float _damage;
        private void OnTriggerEnter2D(Collider2D other) 
        {   
            if(other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }
    }

}
