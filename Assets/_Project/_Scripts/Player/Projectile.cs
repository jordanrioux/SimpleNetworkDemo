using Unity.Netcode;
using UnityEngine;

namespace SimpleNetworkDemo.Player
{
    public class Projectile : MonoBehaviour 
    {
        [SerializeField] private Vector2 _velocity;

        public void Awake() 
        {
            Invoke(nameof(DestroyItself), 5);
        }

        private void FixedUpdate()
        {
            var translation = Vector2.up * _velocity * Time.fixedDeltaTime;
            transform.Translate(translation, Space.Self);
        }

        private void DestroyItself() 
        {
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            DestroyItself();
        }
    }
}
