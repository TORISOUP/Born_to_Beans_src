using UnityEngine;
using System.Collections;
using GGJ.Debugs;
using GGJ.Player;
using GGJ.Utils;
using UniRx;

namespace GGJ.GameManager
{
    /// <summary>
    /// ゲーム中の時間を管理するよ！
    /// </summary>
    public class TimerManager : SingletonMonoBehaviour<TimerManager>
    {

        private ReactiveProperty<float> timeReactiveProperty = new FloatReactiveProperty(0);

        /// <summary>
        /// 試合開始からの秒数を返す
        /// </summary>
        public ReadOnlyReactiveProperty<float> OnGameTimer
        {
            get { return timeReactiveProperty.ToReadOnlyReactiveProperty(); }
        }

        /// <summary>
        /// ゲーム中のみカウントアップする
        /// </summary>
        private IEnumerator CountUpCoroutine()
        {


            //ゲームが始まるのを待つ
            if (DebugSceneMarker.instance == null)
            {
                var gameState = GameState.Instance;
                yield return gameState
                    .GameStateReactiveProperty.FirstOrDefault(x => x == GameStateEnum.GameUpdate)
                    .StartAsCoroutine();

                while (gameState.GameStateReactiveProperty.Value == GameStateEnum.GameUpdate)
                {
                    timeReactiveProperty.Value += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                while (true)
                {
                    timeReactiveProperty.Value += Time.deltaTime;
                    yield return null;
                }
            }


        }

        void Start()
        {
            StartCoroutine(CountUpCoroutine());
        }

    }
}
