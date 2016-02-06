using UnityEngine;
using System.Collections;
using UniRx;

public class RuleSelectPartPresenter : ObservableMonoBehaviour
{
    [SerializeField]
    protected RuleSelectDirectorComponent directorComponent;
    [SerializeField]
    protected RuleSelectDirectorComponent.StateType ownState;
    [SerializeField]
    protected RuleSelectDirectorComponent.StateType preState;
    [SerializeField]
    protected RuleSelectDirectorComponent.StateType nextState;

	// Use this for initialization
	void Awake () {
        directorComponent.currentState.Subscribe(state =>{
            if (state == (int)ownState){
                directorComponent.preState = (int)preState;
                Show();
            }
            else if (state == (int)preState) {
                Back();
            } else if (state == (int)nextState) {
                Next();
            }
            
        }).AddTo(this);
	}

    public virtual void Show()
    {
        
    }
    public virtual void Back()
    {

    }
    public virtual void Next()
    {

    }

}
