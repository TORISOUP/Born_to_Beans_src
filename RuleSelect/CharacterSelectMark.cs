using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GGJ.Player;
using UniRx;

namespace GGJ.RuleSelect
{
    public class CharacterSelectMark : MonoBehaviour
    {
        [SerializeField]
        Image selectMarkImage; 
        [SerializeField, Range(1, 4)]
        int playerNumber;
        public int PlayerNumber { get { return playerNumber; } }
        private MultiPlayerInput currentInput;
        public int selectId = 0;
        public bool IsSelected = false;
        public bool IsEnable { get { return playerNumber <= GameMatchSetting.Instance.CurrentModePlayerLimit; } }

        void Awake()
        {
            currentInput = this.GetComponent<MultiPlayerInput>();
            currentInput.MoveDirection.Subscribe(dir =>
                {
                    if (dir.x > 0)
                        selectId ++;
                    else if (dir.x < 0)
                        selectId --;
                    
                }).AddTo(this);
        }

        public void Initialize()
        {
            IsSelected = false;
        }
    }
}