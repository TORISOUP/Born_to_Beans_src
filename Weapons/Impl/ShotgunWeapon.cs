using System;
using UnityEngine;
using System.Collections;
using GGJ.Bullet;
using UniRx;
using Random = UnityEngine.Random;

namespace GGJ.Weapons
{
    public class ShotgunWeapon : Weapon
    {
        [SerializeField]
        GameObject bullet;

        [SerializeField]
        private float NoizeAngle = 30;

        [SerializeField]
        private float CreateBullesCount = 7;

        [SerializeField]
        private int maxAmmo = 3;
        private int _currentAmmo;
        private int CurrentAmmo
        {
            set
            {
                _currentAmmo = value;
                currentAmmoRate.Value = ((float)_currentAmmo / maxAmmo);
                if (_currentAmmo <= 0) onFinishedSubject.OnNext(Unit.Default);
            }
            get { return _currentAmmo; }
        }

        private void Start()
        {
            CurrentAmmo = maxAmmo;
            this.OnShootAsObservable
                .DistinctUntilChanged()
                .Where(x => x)
                .ThrottleFirst(TimeSpan.FromMilliseconds(500))
                .Subscribe(x => Attack());
        }

        public void Attack()
        {
            var startPosition = MuzzleTransform.position;
            var direction = MuzzleTransform.forward;

            var baseRotation = Quaternion.LookRotation(direction);

            for (int i = 0; i < CreateBullesCount; i++)
            {
                CreateBullet(startPosition, baseRotation);
            }

            PlayShotSe();

            CurrentAmmo--;
        }

        private void CreateBullet(Vector3 startPos, Quaternion baseAngle)
        {

            var targetAngle =
                Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), MuzzleTransform.forward)
                        * Quaternion.AngleAxis(Random.Range(-NoizeAngle, 0), MuzzleTransform.right)
                        * baseAngle;

            var b = Instantiate(bullet, startPos, targetAngle) as GameObject;
            b.GetComponent<BaseBullet>().RegisterAttacker(attacker);

        }
    }
}
