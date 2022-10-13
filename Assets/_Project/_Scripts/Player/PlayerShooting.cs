using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleNetworkDemo.Player
{
    public class PlayerShooting : NetworkBehaviour
    {
        [SerializeField] private Projectile _projectile;
        [SerializeField] private AudioClip _projectileSound;
        [SerializeField] private float _cooldown = 0.5f;
        [SerializeField] private Transform _projectileSpawner;

        private float _lastFired = float.MinValue;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && _lastFired + _cooldown < Time.time)
            {
                _lastFired = Time.time;
                CreateProjectileServerRpc();
                Shoot();
            }
        }

        [ServerRpc]
        private void CreateProjectileServerRpc()
        {
            Assert.IsTrue(IsServer, "Can only be run by server");
            SpawnProjectileClientRpc();
        }

        [ClientRpc]
        private void SpawnProjectileClientRpc()
        {
            if (!IsOwner)
            {
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
