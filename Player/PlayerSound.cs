using UnityEngine;
using System.Collections;
using UniRx;

namespace GGJ.Player {
    
    public class PlayerSound : MonoBehaviour
    {
        [SerializeField]
        PlayerCore currentPlayer;
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip hitClip;

    	// Use this for initialization
    	void Awake () {
            if (currentPlayer == null)
                currentPlayer = this.GetComponent<PlayerCore>();
            if (currentPlayer != null)
            {
                currentPlayer.OnPlayerDamagedObservable.Subscribe(_ =>
                    {
                        audioSource.PlayOneShot(hitClip);
                    }).AddTo(this);
            }
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}
    }
}