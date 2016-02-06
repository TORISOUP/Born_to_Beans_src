using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using GGJ.Damages;
using GGJ.Items;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using GGJ.GameManager;
using System.Linq;
using GGJ.Utils;

namespace GGJ.Player
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        private ReactiveCollection<PlayerCore> players = new ReactiveCollection<PlayerCore>();
        private List<int> deadPlayers = new List<int>();

        private IConnectableObservable<PlayerCore> onPlayerSpawned;

        /// <summary>
        /// プレイヤが登録されたことを通知する
        /// </summary>
        public IObservable<PlayerCore> OnPlayerSpawnedAsObservable
        {
            get
            {
                return onPlayerSpawned;
            }
        }

        private Subject<PlayerCore> _playerWinner = new Subject<PlayerCore>();
        public IObservable<PlayerCore> OnWinnerPlayerAsObservable()
        {
            return _playerWinner.AsObservable();
        }

        private Subject<PlayerCore> onPlayerDeadSubject = new Subject<PlayerCore>();

        public IObservable<PlayerCore> OnPlayerDeadObservable
        {
            get { return onPlayerDeadSubject.AsObservable(); }
        }

        public void SetPlayer(PlayerCore player)
        {
            // 多重登録禁止
            if (players.ToList().Exists(x => x.PlayerId == player.PlayerId))
                return;
            // 作られたプレイヤーを管理リストに追加
            players.Add(player);

            //プレイヤが死んだら
            player.OnPlayerDeadAsObservable
                .FirstOrDefault()
                .Subscribe(x =>
                {
                    DeadPlayer(x);
                });
        }

        /// <summary>
        /// PlayerIdからPlayerCoreを取得
        /// </summary>
        public PlayerCore FindPlayer(int id)
        {
            return players.FirstOrDefault(x => x.PlayerId == id);
        }

        private void Awake()
        {
            //過去に発行した値をすべて保存しておく
            onPlayerSpawned = players
                    .ObserveAdd()
                    .Select(x => x.Value)
                    .Replay();
            onPlayerSpawned.Connect();
        }

        /// <summary>
        /// 生きているプレイヤー一覧を返す
        /// </summary>
        public List<PlayerCore> GetAlivePlayers()
        {
            return players
            .Where(x => (deadPlayers.Exists(y => y == x.PlayerId) == false))
             .ToList();
        }

        /// <summary>
        /// 死んでいるプレイヤー一覧を返す
        /// </summary>
        public List<PlayerCore> GetDeadPlayers()
        {
            return deadPlayers.Select(x => FindPlayer(x)).ToList();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
        }

        private void DeadPlayer(int id)
        {
            if (GetAlivePlayers().Count <= 1)
                return;

            var player = FindPlayer(id);

            players.Remove(player);
            onPlayerDeadSubject.OnNext(player);

            if (players.Count == 1)  // 残り一人になったら
            {
                // 勝者のPlayerCoreを通知
                _playerWinner.OnNext(players[0]);
                _playerWinner.OnCompleted();
            }
        }
    }
}
