using UnityEngine;
using System.Collections;
using GGJ.Damages;
using GGJ.Player;

namespace GGJ.Bullet
{
    public class PlayerAttachedBullet : BaseBullet
    {
        [SerializeField]
        private GameObject parent;
        private IDamageable myPlayer;
        private PlayerMover mover;
        protected override void OnStart()
        {
            this.attacker = parent.GetComponent<IAttacker>();
            myPlayer = parent.GetComponent<IDamageable>();

            //TODO:ちゃんとインターフェイスに切り分ける
            mover = parent.GetComponent<PlayerMover>();
        }

        protected override void Hit(GameObject hitTarget)
        {
            var hitAttacker = hitTarget.GetComponent<IAttacker>();

            if (hitAttacker != null && hitAttacker.AttackerId == attacker.AttackerId)
            {
                return;
            }

            var damageable = hitTarget.GetComponent<IDamageable>();
            if (damageable == null) return;

            var dir = (hitTarget.transform.position - this.transform.position).normalized;
            //自分の現在速度をそのまま相手に伝える、速度が低すぎる場合はデフォルト値を伝える
            var targetPower = Mathf.Max(_power, mover.CurrentMoveVelocity.magnitude);

            //相手にぶつかった時
            var damage = new Damage(dir, targetPower, attacker);
            damageable.ApplyDamage(damage);

            //自分にもダメージ
            var myDamage = Mathf.Max(_power, targetPower / 2.0f);
            var damage2 = new Damage(-mover.CurrentMoveVelocity.normalized, myDamage, attacker);
            myPlayer.ApplyDamage(damage2);

            PlayEffect();
            PlayShotHit();
        }
    }
}
