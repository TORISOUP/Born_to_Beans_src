using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GGJ.Debugs;
using GGJ.Player;
using UniRx;

namespace GGJ.GameManager
{
    /// <summary>
    /// ゲームのリザルトで用いる情報を管理する
    /// </summary>
    public class ResultStateManager : MonoBehaviour
    {
        [SerializeField]
        private ResultPresenter presenter;

        private List<ResultInfo> resultPlayerList = new List<ResultInfo>();

        void Start()
        {
            var timer = TimerManager.Instance;

            //ゲーム終了通知
            var gameFinish = DebugSceneMarker.Instance != null
                ? PlayerManager.Instance.OnWinnerPlayerAsObservable().AsUnitObservable()
                : GameState.Instance.GameStateReactiveProperty.Where(x => x == GameStateEnum.Judge).AsUnitObservable();

            PlayerManager.Instance.OnPlayerDeadObservable
                .TakeUntil(gameFinish)
                .Subscribe(p =>
                {
                    //プレイヤが死んだらリストに追加
                    resultPlayerList.Insert(0, new ResultInfo(
                            p,
                            timer.OnGameTimer.Value
                        ));
                });

            //ゲームが終わったら表示
            gameFinish.FirstOrDefault()
                .Subscribe(_ =>
                {
                    var winner = PlayerManager.Instance.GetAlivePlayers().FirstOrDefault();
                    presenter.ShowResult(resultPlayerList, winner);

                });
        }

    }

    public class ResultInfo
    {
        public PlayerCore PlayerCore { get; private set; }
        public float DeadTime { get; private set; }

        public ResultInfo(PlayerCore core, float time)
        {

            PlayerCore = core;
            DeadTime = time;
        }
    }
}
