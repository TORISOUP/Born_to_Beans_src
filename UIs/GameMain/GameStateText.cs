using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GGJ.GameManager;
using UniRx;
using UniRx.Triggers;

namespace GGJ.UIs
{
    [RequireComponent(typeof(Text))]
    public class GameStateText : MonoBehaviour 
    {
        Text _textComp = null;
        Text TextComp {get{ return _textComp ? _textComp : _textComp = GetComponent<Text>(); }}
        // Use this for initialization
        void Start () 
        {
            GameState.Instance.CountDownReactiveProperty
            .Subscribe( x =>{
                if( x != 0)
                    TextComp.text = x.ToString();
            });
            
            GameState.Instance.GameStateReactiveProperty
            .Subscribe( x => {
                if( x == GameStateEnum.Standby )
                {
                    TextComp.text = "Ready";
                }
                if( x == GameStateEnum.GameUpdate )
                {
                    OnStateStart();
                }
            });
        }
        
        void OnStateStart()
        {
            TextComp.text = "Start";
            Observable.Timer( System.TimeSpan.FromSeconds(1))
            .Take(1)
            .Subscribe( _=> {
                    TextComp.text = "";
            });
        }
    }
}
