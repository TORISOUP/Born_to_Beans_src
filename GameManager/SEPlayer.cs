using UnityEngine;
using System.Collections;
using GGJ.Utils;

namespace GGJ.GameManager
{

    [RequireComponent(typeof(AudioSource))]
    public class SEPlayer : SingletonMonoBehaviour<SEPlayer>
    {
        [SerializeField]
        private AudioSource audioSource;

        public void PlaySe(AudioClip clip)
        {

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            audioSource.PlayOneShot(clip);
        }
    }
}