using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace GGJ.GameManager
{
    public class SoundManager : MonoBehaviour
    {

        // BGM AudioSource
        [SerializeField]
        private AudioSource bgmSource;

        // BGM
        public AudioClip bgm;

        void Awake (){

            // BGM Loop
            bgmSource.loop = true;
        }

        // Use this for initialization
        void Start()
        {
            GameState.Instance.GameStateReactiveProperty.Where(x => x == GameStateEnum.GameUpdate).Subscribe(_ =>
                {
                    bgmSource.Stop();
                    bgmSource.clip = bgm;
                    bgmSource.Play();
                });
        }
    }
}
