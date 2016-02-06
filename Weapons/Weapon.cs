using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using GGJ.Damages;
using GGJ.GameManager;
using GGJ.RuleSelect;

namespace GGJ.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        protected AudioClip ShotSE;

        protected IAttacker attacker;

        public string WeaponName;

        protected ReactiveProperty<float> currentAmmoRate = new FloatReactiveProperty(1.0f);

        protected void PlayShotSe()
        {
            if (ShotSE == null) return;
            SEPlayer.Instance.PlaySe(ShotSE);
        }

        /// <summary>
        /// 武器の現在の残弾数を1.0f-0.0fのfloatで表現する
        /// </summary>
        public ReadOnlyReactiveProperty<float> CurrentAmmoRate
        {
            get { return currentAmmoRate.ToReadOnlyReactiveProperty(); }
        }

        public void Initializer(IAttacker attacker, IObservable<bool> attackObservable)
        {
            this.attacker = attacker;
            this.OnShootAsObservable = attackObservable.TakeUntil(this.OnDestroyAsObservable());
        }

        protected IObservable<bool> OnShootAsObservable;

        /// <summary>
        /// 武器を使い終わった時に通知する
        /// </summary>
        public IObservable<Unit> OnFinishedAsync
        {
            get { return onFinishedSubject.FirstOrDefault().AsObservable(); }
        }
        protected Subject<Unit> onFinishedSubject = new Subject<Unit>();

        private Transform _muzzleTransform;
        protected Transform MuzzleTransform
        {
            get
            {
                if (_muzzleTransform != null) return _muzzleTransform;
                var muzzle = GetComponentInChildren<Muzzle>();
                if (muzzle == null) Debug.LogWarning("Muzzle not seted!");
                _muzzleTransform = muzzle != null ? muzzle.transform : this.transform;
                return _muzzleTransform;
            }
        }

        /// <summary>
        /// 武器の種類
        /// </summary>
        public WeaponEnum WeaponType { get; protected set; }

    }
}
