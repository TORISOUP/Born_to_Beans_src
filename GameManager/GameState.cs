using UnityEngine;
using System.Collections;
using GGJ.Utils;
using UniRx;
using GGJ.Player;

namespace GGJ.GameManager
{
    public enum GameStateEnum
    {
        Standby,
        Countdown,
        GameUpdate,
        Judge,
        Exit
    }


    /// <summary>
    /// ゲーム全体の進行状態を管理する
    /// </summary>
    [DisallowMultipleComponent]
    public class GameState : SingletonMonoBehaviour<GameState>
    {
        [SerializeField]
        private PlayerManager playerManager;
        
        // ゲーム状態
        private ReactiveProperty<GameStateEnum> _gameState = new ReactiveProperty<GameStateEnum>(GameStateEnum.Standby);
       
        /// <summary>
        /// ゲームの状態
        /// </summary>
        public IReadOnlyReactiveProperty<GameStateEnum> GameStateReactiveProperty { get { return _gameState.ToReadOnlyReactiveProperty(); }}
        
        private ReactiveProperty<int> _gameCount = new ReactiveProperty<int>();
        public IReadOnlyReactiveProperty<int> CountDownReactiveProperty { get { return _gameCount.ToReadOnlyReactiveProperty(); }}
        
        // ステージ管理者
        [SerializeField]
        private StageSpawner m_StageSpawner;

        void Start()
        {
            // フレームレートを60に変更する
            Application.targetFrameRate = 60;

            // ゲームが終了したらタイトルに戻す処理
            _gameState.Where(x => x == GameStateEnum.Exit)
                .First()
                .Subscribe(_ =>
                {
                    // ゲームが終了されたら次のシーンに行く
                    FadeSceneTransition.instance.FadeChangeScene(SceneNameEnum.Title);
                });

            // ゲーム進行開始
            StartCoroutine(GameStart());
        }

        /// <summary>
        /// ゲーム進行
        /// </summary>
        private IEnumerator GameStart()
        {
            //生成待機
            yield return m_StageSpawner.OnStageInitCompleteAsObservable().FirstOrDefault().StartAsCoroutine();
            
            //フェードが終わるのを待機
            yield return FadeSceneTransition.instance.IsFading.Where(x => x == false).FirstOrDefault().StartAsCoroutine();
            
            // フェード終了後ちょっとだけ待つ
            yield return new WaitForSeconds(0.5f);

            // カウントダウン開始
            _gameState.Value = GameStateEnum.Countdown;
            yield return StartCoroutine(StartCountDown(3)); // 終了待ち
                
            // ゲームメイン処理
            _gameState.Value =GameStateEnum.GameUpdate;
            yield return playerManager.OnWinnerPlayerAsObservable().StartAsCoroutine();
            
            // 結果表示
            _gameState.Value = GameStateEnum.Judge;
            yield return new WaitForSeconds(5); // 終了待ち
            
            // ゲーム終了状態へ
            _gameState.Value =GameStateEnum.Exit;
            yield break;
        }
        
        private IEnumerator StartCountDown( int startCount = 3 )
        {
            _gameCount.Value = startCount ;
            while(_gameCount.Value > 0)
            {
                yield return new WaitForSeconds( 1f );
                _gameCount.Value -= 1;
            }
            
            yield return 0;
        }
        
    }
}
