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

        [SerializeField] private GameObject _bombPrefab;
        
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
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateBombServerRpc();
            }
        }

        [ServerRpc]
        private void CreateBombServerRpc()
        {
            var t = transform;
            var bomb = Instantiate(_bombPrefab, t.position, t.rotation);
            bomb.GetComponent<NetworkObject>().Spawn();
        }

        [ServerRpc]
        private void CreateProjectileServerRpc()
        {
            Shoot();
        }

        private void Shoot()
        {
            var t = _projectileSpawner.transform;
            var projectile = Instantiate(_projectile, _projectileSpawner.position, _projectileSpawner.rotation);
            
            var networkObject = projectile.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Spawn();
            }

            PlaySoundClientRpc();
        }

        [ClientRpc]
        private void PlaySoundClientRpc()
        {
            AudioSource.PlayClipAtPoint(_projectileSound, transform.position);
        }
    }
}
