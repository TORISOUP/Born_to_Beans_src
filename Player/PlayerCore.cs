using System;
using GGJ.Damages;
using GGJ.Debugs;
using GGJ.Items;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GGJ.Player
{
    /// <summary>
    /// プレイヤのコア
    /// </summary>
    public class PlayerCore : MonoBehaviour, IAttacker, IDamageable
    {
        /// <summary>
        /// プレイヤ識別子
        /// </summary>
        public int PlayerId
        {
            get { return _playerId; }
        }

        [SerializeField]
        private int _playerId = 1;

        /// <summary>
        /// プレイヤの名前
        /// </summary>
        public string PlayerName
        {
            get { return string.Format("{0}P", PlayerId); }
        }

        #region ItemType
        private Subject<ItemType> onPickUpSubject;
        /// <summary>
        /// プレイヤがアイテムを取得したことを通知する
        /// </summary>
        public IObservable<ItemType> OnPickUpItemObservable
        {
            get
            {
                if (onPickUpSubject == null) onPickUpSubject = new Subject<ItemType>();
                return onPickUpSubject.AsObservable();
            }
        }
        #endregion
        #region PlayerDamage
        private Subject<Damage> onPlayerDamageSubject;

        /// <summary>
        /// プレイヤがダメージを受けたことを通知する
        /// </summary>
        public IObservable<Damage> OnPlayerDamagedObservable
        {
            get
            {
                if (onPlayerDamageSubject == null) onPlayerDamageSubject = new Subject<Damage>();
                return onPlayerDamageSubject.AsObservable();
            }
        }

        /// <summary>
        /// プレイヤが操作可能な状態であるか
        /// </summary>
        public ReactiveProperty<bool> PlayerControllable = new BoolReactiveProperty(false);

        /// <summary>
        /// プレイヤにダメージを与える
        /// </summary>
        /// <param name="damage"></param>
        public void ApplyDamage(Damage damage)
        {
            if (onPlayerDamageSubject != null)
            {
                onPlayerDamageSubject.OnNext(damage);
            }
        }

        #endregion
        #region IAttacker
        public string AttackerId
        {
            get { return "Player_" + PlayerId; }
        }

        public string AttackerName
        {
            get { return PlayerName; }
        }
        #endregion

        #region Dead

        ReactiveProperty<bool> playerAliveReactiveProperty = new BoolReactiveProperty(true);

        /// <summary>
        /// プレイヤが死亡したことを通知する
        /// </summary>
        public IObservable<int> OnPlayerDeadAsObservable
        {
            get { return playerAliveReactiveProperty.Where(x => !x).Select(_ => PlayerId); }
        }

        #endregion

        public void SetPlayerId(int id)
        {
            _playerId = id;
            //debug
            PlayerManager.Instance.SetPlayer(this);
        }


        void Start()
        {
            PlayerControllable.Value = true;

            if (DebugSceneMarker.Instance != null)
            {
                SetPlayerId(_playerId);
                Debug.Log("デバッグ用にPlayerIDを手動設定してます！注意！ " + _playerId);
            }

            //アイテムを拾って通知する
            this.OnTriggerEnterAsObservable()
                .Select(x => x.gameObject.GetComponent<IItemObject>())
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    if (onPickUpSubject != null)
                    {
                        onPickUpSubject.OnNext(x.ItemType);
                    }
                    x.PickUp();
                });

            //ダメージを受けたら操作不能状態にする
            OnPlayerDamagedObservable
                .Do(_ => PlayerControllable.Value = false)
                .Delay(TimeSpan.FromMilliseconds(300))
                .Subscribe(_ => PlayerControllable.Value = true);

            this.transform.ObserveEveryValueChanged(x => x.position)
                .Where(p => p.y < -2)
                .FirstOrDefault()
                .Subscribe(_ =>
                {
                    playerAliveReactiveProperty.Value = false;
                }).AddTo(this.gameObject);

        }

    }
}
