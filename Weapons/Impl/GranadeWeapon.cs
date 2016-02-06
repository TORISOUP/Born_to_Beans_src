using System;
using UnityEngine;
using System.Collections;
using GGJ.Bullet;
using GGJ.Weapons;
using UniRx;
using Random = UnityEngine.Random;

namespace GGJ.Weapons
{

    public class GranadeWeapon : Weapon
    {

        [SerializeField] private GameObject bullet;

        [SerializeField] private int maxAmmo = 50;

        [SerializeField] private float NoizeAngle = 10;

        private int _currentAmmo;

        private int CurrentAmmo
        {
            set
            {
                _currentAmmo = value;
                currentAmmoRate.Value = ((float) _currentAmmo/maxAmmo);
                if (_currentAmmo <= 0) onFinishedSubject.OnNext(Unit.Default);
            }
            get { return _currentAmmo; }
        }

        private void Start()
        {
            CurrentAmmo = maxAmmo;
            this.OnShootAsObservable
                .Where(x => x && CurrentAmmo > 0)
                .ThrottleFirst(TimeSpan.FromMilliseconds(600))
                .Subscribe(_ => Attack());
        }


        public void Attack()
        {
            var direction = MuzzleTransform.forward;
            var startPosition = MuzzleTransform.position;

            var dirQua = Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), MuzzleTransform.forward)
                         *Quaternion.AngleAxis(Random.Range(-NoizeAngle, 0), MuzzleTransform.right)
                         *Quaternion.LookRotation(direction);

            var b = Instantiate(bullet, startPosition, dirQua) as GameObject;
            b.GetComponent<BaseBullet>().RegisterAttacker(attacker);

            CurrentAmmo--;

            PlayShotSe();
        }
    }
}
