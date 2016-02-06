using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

namespace GGJ.RuleSelect
{
    public class PlayerModeSelectPresenter : RuleSelectPartPresenter
    {
        RectTransform rectTransform;
        void Start()
        {
            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
            rectTransform = this.GetComponent<RectTransform>();
        }

        public void SelectMode(GameMatchSetting.PlayMode mode)
        {
            GameMatchSetting.Instance.SetPlayMode(mode);
            this.directorComponent.currentState.Value = (int)nextState;
            directorComponent.PlaySound(RuleSelectDirectorComponent.SoundType.Enter);
        }

        public override void Next()
        {
            base.Next();
            rectTransform.DOLocalMoveX(-800,1.0f);
        }

        public override void Back()
        {
            base.Back();
        }

        public override void Show()
        {
            base.Show();
            rectTransform.DOLocalMoveX(0,1.0f);
        }

    }
}