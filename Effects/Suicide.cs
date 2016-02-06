using UnityEngine;
using System.Collections;

namespace GGJ.Effects
{

    public class Suicide : MonoBehaviour
    {
        [SerializeField] private float deadTime = 1;

        void Start()
        {
            Destroy(this.gameObject,deadTime);
        }

    }
}
