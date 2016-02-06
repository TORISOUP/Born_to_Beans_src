using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using GGJ.Player;
using System.Collections.Generic;
using UniRx;
using System.Linq;

namespace GGJ.RuleSelect
{

    public class CharacterSelectPresenter : RuleSelectPartPresenter
    {
        RectTransform rectTransform;
        [SerializeField]
        List<CharacterSelectIcon> characterIcons = new List<CharacterSelectIcon>();
        [SerializeField]
        List<CharacterSelectMark> playerMarks = new List<CharacterSelectMark>();

        void Start()
        {
            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
            rectTransform = this.GetComponent<RectTransform>();
        }

        public void CharacterDone()
        {
            this.directorComponent.currentState.Value = (int)nextState;
        }

        public override void Next()
        {
            base.Next();
            rectTransform.DOLocalMoveX(-1000,1.0f);
        }

        public override void Back()
        {
            base.Back();
            rectTransform.DOLocalMoveX(1000,1.0f);
        }

        public override void Show()
        {
            base.Show();
            rectTransform.DOLocalMoveX(0,1.0f);
            this.playerMarks.ForEach(item => item.gameObject.SetActive(item.PlayerNumber <= GameMatchSetting.Instance.CurrentModePlayerLimit));
        }

    }
}