using Unity.Netcode;
using UnityEngine;

namespace SimpleNetworkDemo.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Projectile _projectile;
        [SerializeField] private AudioClip _projectileSound;
        [SerializeField] private float _cooldown = 0.5f;
        [SerializeField] private Transform _projectileSpawner;

        private float _lastFired = float.MinValue;

        private void Update()
        {
            if (Input.GetMouseButton(0) && _lastFired + _cooldown < Time.time)
            {
                _lastFired = Time.time;
                Shoot();
            }
        }

        private void Shoot()
        {
            var t = _projectileSpawner.transform;
            var projectile = Instantiate(_projectile, _projectileSpawner.position, _projectileSpawner.rotation);
            AudioSource.PlayClipAtPoint(_projectileSound, transform.position);
        }
    }
}
