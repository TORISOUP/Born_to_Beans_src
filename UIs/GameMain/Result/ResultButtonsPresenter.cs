using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using GGJ.Utils;
using GGJ.RuleSelect;

namespace GGJ.UIs {

    public class ResultButtonsPresenter : MonoBehaviour {

        //次のステージへボタン
        [SerializeField]
        private Button nextStageButton;
        //タイトルへ戻る
        [SerializeField]
        private Button ruleSelectButton;
    	// Use this for initialization
        void Start () {
            ruleSelectButton.OnClickAsObservable().Subscribe(_ =>
                {
                    FadeSceneTransition.Instance.FadeChangeScene(SceneNameEnum.RuleSelect);
                }).AddTo(this);
            nextStageButton.OnClickAsObservable().Subscribe(_ =>
                {
                    GameMatchSetting.Instance.SetupNextStage();
                    FadeSceneTransition.Instance.FadeChangeScene(SceneNameEnum.GameMain);
                }).AddTo(this);
    	
    	}
    }
}
