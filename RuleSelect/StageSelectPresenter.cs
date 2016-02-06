using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using GGJ.RuleSelect;
using GGJ.Stage;

public class StageSelectPresenter : RuleSelectPartPresenter
{
    RectTransform rectTransform;
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        rectTransform = this.GetComponent<RectTransform>();
    }

    public void StageSelectDone()
    {
        GameMatchSetting.Instance.SetStage(StageEnum.Plain);
        this.directorComponent.currentState.Value = (int)nextState;
        this.directorComponent.PlaySound(RuleSelectDirectorComponent.SoundType.Enter);
    }
    public void StageSelect(StageEnum id)
    {
        GameMatchSetting.Instance.SetStage(id);
        this.directorComponent.currentState.Value = (int)nextState;
        this.directorComponent.PlaySound(RuleSelectDirectorComponent.SoundType.Enter);
    }

    public override void Next()
    {
        base.Next();
    }

    public override void Back()
    {
        base.Back();
        rectTransform.DOLocalMoveX(800,1.0f);
    }

    public override void Show()
    {
        base.Show();
        rectTransform.DOLocalMoveX(0,1.0f);
    }

}
