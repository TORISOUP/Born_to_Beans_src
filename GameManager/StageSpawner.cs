using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using GGJ.Items;
using GGJ.Player;
using GGJ.Stage;
using GGJ.UIs;
using GGJ.RuleSelect;
using System.Collections.Generic;
using System.Linq;

namespace GGJ.GameManager
{
    /// <summary>
    /// ステージにアイテムとプレイヤーを生成しているもの
    /// </summary>
    public class StageSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] itemPrefab;

        [SerializeField]
        private GameObject[] stagePrefab;

        [SerializeField]
        private GameObject[] playerPrefab;

        [SerializeField]
        private PlayerManager playerManager;

        [SerializeField]
        private StageCameraPotision stageCamera;

        private StageCore stageCore;

        /// <summary>
        /// 生成したアイテム一覧
        /// </summary>
        private Dictionary<int, GameObject> spownedItemDictionary = new Dictionary<int, GameObject>();

        private List<Transform> spownItemPositions = null;

        private Subject<Unit> onStageInitComplete = new Subject<Unit>();
        /// <summary>
        /// ステージの初期化完了通知
        /// </summary>
        public IObservable<Unit> OnStageInitCompleteAsObservable()
        {
            return onStageInitComplete.AsObservable();
        }

        // Use this for initialization
        void Start()
        {
            // 準備完了
            GameState.Instance.GameStateReactiveProperty
            .Where(x => x == GameStateEnum.Standby)
            .FirstOrDefault()
            .Subscribe(_ =>
            {
                StartCoroutine(InitGame());
            });

            // ランダム生成
            GameState.Instance.GameStateReactiveProperty
            .Where(x => x == GameStateEnum.GameUpdate)
            .FirstOrDefault()
            .Subscribe(_ =>
            {
                StartCoroutine("SpawnRandomItem");
            });

        }
        /// <summary>
        /// ステージ初期化
        /// </summary>
        IEnumerator InitGame()
        {
            yield return SpawnStage();
            yield return SpawnPlayers();

            onStageInitComplete.OnNext(Unit.Default);
            onStageInitComplete.OnCompleted();
            // 初期化完了通知
            yield break;
        }

        /// <summary>
        /// ステージPrefabから読み込み
        /// </summary>
        IEnumerator SpawnStage()
        {
            var createStageId = (int)GameMatchSetting.Instance.SelectedStageType;
            var stageObj = Instantiate(stagePrefab[createStageId]) as GameObject;
            stageObj.transform.position = Vector3.zero;
            stageCore = stageObj.GetComponent<StageCore>();
            stageCamera.SetPosition(stageCore.GetCameraPosition(), stageCore.GetStageCameraAngle());

            spownItemPositions = stageCore.GetRandomItemSpownPosition();
            yield break;
        }

        /// <summary>
        /// プレイヤー生成
        /// </summary>
        IEnumerator SpawnPlayers()
        {
            var createCount = GameMatchSetting.Instance.CurrentModePlayerLimit;

            // yield return new WaitForSeconds(1);

            var createPositionList = stageCore.GetRandomSpownPosition(createCount);

            playerPrefab[0].gameObject.SetActive(false);
            for (var i = 0; i < createCount; ++i)
            {
                var playerData = Instantiate(playerPrefab[0]) as GameObject;
                var corePlayer = playerData.GetComponent<PlayerCore>();
                playerData.transform.position = createPositionList[i].position;
                playerData.transform.rotation = createPositionList[i].rotation;
                corePlayer.SetPlayerId(i + 1);
                playerData.GetComponent<PlayerColor>().SetPlayerColor(GetPlayerColor(corePlayer.PlayerId));
                playerData.GetComponent<MultiPlayerInput>().playerNumber = i + 1;
                playerData.SetActive(true);
                playerManager.SetPlayer(corePlayer);
            }

            // 初期化開始
            playerManager.Init();

            yield break;
        }

        Color GetPlayerColor(int id)
        {
            switch (id)
            {
                case 1:
                    return Color.red;
                case 2:
                    return Color.blue;
                case 3:
                    return Color.green;
                case 4:
                    return Color.yellow;
            }

            return Color.black;
        }

        /// <summary>
        /// プレイヤー生成
        /// </summary>
        IEnumerator SpawnRandomItem()
        {
            var waitTime = 10;
            while (GameState.Instance.GameStateReactiveProperty.Value == GameStateEnum.GameUpdate)
            {
                yield return new WaitForSeconds(waitTime);

                // ランダムで生成位置を取得する
                var randomId = Random.Range(0, spownItemPositions.Count);

                //アイテムがすでに存在する場所には生成しない
                var targetObject = spownedItemDictionary.FirstOrDefault(x => x.Key == randomId);
                if (targetObject.Value != null)
                {
                    continue;
                }

                //ランダムにアイテム生成
                var itemId = Random.Range(0, itemPrefab.Length);
                var spownItem = Instantiate(itemPrefab[itemId], spownItemPositions[randomId].position, spownItemPositions[randomId].rotation) as GameObject;

                //要素を上書き
                spownedItemDictionary[randomId] = spownItem;

                //だんだん生成頻度を短くする
                waitTime = Mathf.Max(5, waitTime - 1);
            }
        }
    }
}
