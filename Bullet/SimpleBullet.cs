using UnityEngine;
using System.Collections;
using GGJ.Damages;
using UniRx;
using UniRx.Triggers;
using GGJ.Damages;

namespace GGJ.Bullet
{
    public class SimpleBullet : BaseBullet
    {
        Rigidbody rigidBody;
        [SerializeField]
        private float lifeTime = 3.0f;

        [SerializeField] private bool IsHitToAttakcer = false;

        protected override void OnStart()
        {

            rigidBody = GetComponent<Rigidbody>();

            this.FixedUpdateAsObservable().First().Subscribe(_ =>
            {
                rigidBody.velocity = transform.forward * _speed;
            });

            Destroy(this.gameObject, lifeTime);
        }

        protected override void Hit(GameObject hitTarget)
        {
            var hitAttacker = hitTarget.GetComponent<IAttacker>();

            if (!IsHitToAttakcer && hitAttacker != null && hitAttacker.AttackerId == attacker.AttackerId)
            {
                return;
            }

            var damageable = hitTarget.GetComponent<IDamageable>();
            if (damageable != null)
            {
                var dir = transform.forward;
                var damage = new Damage(dir, _power, attacker);
                damageable.ApplyDamage(damage);
                PlayEffect();
                PlayShotHit();
            }
            
            Destroy(gameObject);
        }

    }
}
