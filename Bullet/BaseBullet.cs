using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using GGJ.Damages;
using GGJ.GameManager;

namespace GGJ.Bullet
{
    public abstract class BaseBullet : MonoBehaviour
    {
        [SerializeField]
        protected float _speed = 5;

        [SerializeField]
        protected float _power = 10;

        [SerializeField]
        private AudioClip[] hitSes;

        [SerializeField] protected GameObject effect;

        protected void PlayShotHit()
        {
            if (!hitSes.Any()) return;
            SEPlayer.Instance.PlaySe(hitSes[Random.Range(0, hitSes.Length)]);
        }

        protected IAttacker attacker;

        public void RegisterAttacker(IAttacker attacker)
        {
            this.attacker = attacker;
        }

        protected void Start()
        {

            this.OnTriggerEnterAsObservable()
                .Subscribe(hit =>
                {
                    //弾同士の接触は無視
                    if (hit.GetComponent<BaseBullet>() != null) return;
                    Hit(hit.gameObject);
                });
            OnStart();
        }

        protected void PlayEffect()
        {
            if(effect==null) return;
            Instantiate(effect, transform.position, Quaternion.identity);
        }

        protected abstract void OnStart();

        protected abstract void Hit(GameObject hit);
    }
}
