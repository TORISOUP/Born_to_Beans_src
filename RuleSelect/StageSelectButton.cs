using UnityEngine;
using System.Collections;
using GGJ.Stage;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField]
    StageSelectPresenter stageSelectPresenter;
    public StageEnum StageId;

    public void Invoke()
    {
        stageSelectPresenter.StageSelect(StageId);
    }
}
