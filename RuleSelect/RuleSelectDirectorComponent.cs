using UnityEngine;
using System.Collections;
using GGJ.Utils;
using UniRx;
using UnityEngine.SceneManagement;

public class RuleSelectDirectorComponent : MonoBehaviour
{
    public enum SoundType
    {
        Enter,
        Select,
        Back
    }
    public enum StateType
    {
        Title,
        ModeSelect,
        CharacterSelect,
        StageSelect,
        GoToMain,
    }

    public int  preState { get; set; }
    public ReactiveProperty<int> currentState = new ReactiveProperty<int>((int)StateType.ModeSelect);
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip enterClip;
    [SerializeField]
    AudioClip selectClip;
    [SerializeField]
    AudioClip modoruClip;

    private void Start()
    {
        currentState.Subscribe(state =>
        {
            if (state == (int) StateType.GoToMain)
                FadeSceneTransition.Instance.FadeChangeScene(SceneNameEnum.GameMain);
        }).AddTo(this);
    }

    public void OnBack()
    {
        currentState.Value = preState;
        PlaySound(SoundType.Back);
        if (currentState.Value <= (int)StateType.Title)
            SceneTransition.ChangeScene(SceneNameEnum.Title);
    }

    public void PlaySound(SoundType type)
    {
        AudioClip clip = enterClip;
        switch (type)
        {
            case SoundType.Enter:
                clip = enterClip;
                break;
            case SoundType.Select:
                clip = selectClip;
                break;
            case SoundType.Back:
                clip = modoruClip;
                break;
        }
        audioSource.PlayOneShot(clip);
    }
}
