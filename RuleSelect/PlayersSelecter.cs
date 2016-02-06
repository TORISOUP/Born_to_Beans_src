using System;
using UnityEngine;
using System.Collections;
using GGJ.RuleSelect;
using UniRx;
using UnityEngine.UI;

namespace GGJ.UIs
{


    public class PlayersSelecter : MonoBehaviour
    {
        [SerializeField]
        private PlayerModeSelectPresenter selecter;

        [SerializeField]
        private GameMatchSetting.PlayMode mode;

        // Use this for initialization
        void Start()
        {
            GetComponent<Button>()
                .OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    selecter.SelectMode(mode);
                    MultiMenuSelectController.Instance.Next();
                });
        }

    }
}
