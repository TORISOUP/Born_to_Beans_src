using System;
using UnityEngine;
using System.Collections;
using GGJ.Bullet;
using UniRx;

namespace GGJ.Weapons
{
    public class SingleShotWeapon : Weapon
    {
        [SerializeField]
        GameObject bullet;

        private void Start()
        {

            this.OnShootAsObservable
                .Where(x => !x)
                .FirstOrDefault()
                .Subscribe(_ =>
                {

                    this.OnShootAsObservable
                        .DistinctUntilChanged()
                        .Where(x => x)
                        .ThrottleFirst(TimeSpan.FromSeconds(0.4f))
                        .Subscribe(__ => Attack());
                });
        }


        public void Attack()
        {

            var startPosition = MuzzleTransform.position;
            var direction = MuzzleTransform.forward;
            var b = Instantiate(bullet, startPosition, Quaternion.LookRotation(direction)) as GameObject;
            b.GetComponent<BaseBullet>().RegisterAttacker(attacker); ;

            PlayShotSe();
        }
    }
}
