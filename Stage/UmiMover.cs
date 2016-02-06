using UnityEngine;
using System.Collections;
using DG.Tweening; 
namespace GGJ.Stage
{
public class UmiMover : MonoBehaviour {
    float scrollSpeed = 0.02F;
    public Renderer rend;

    public float Num {
        set {
            scrollSpeed = value;
        }
        get {
            return scrollSpeed;
        }
    }
    
    void Start() {
        rend = GetComponent<Renderer>();
        
        DOTween.To (() => Num, (n) => Num = n, -0.02f, 30f).SetEase (Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    void Update() {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}

}