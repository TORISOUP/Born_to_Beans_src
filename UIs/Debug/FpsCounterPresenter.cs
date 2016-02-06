using UnityEngine;
using System.Collections;
using GGJ.Utils;
using UniRx;
using UnityEngine.UI;

public class FpsCounterPresenter : MonoBehaviour
{

    void Start()
    {
        var text = GetComponent<Text>();
        FPSCounter.Current.SubscribeToText(text,x=>x.ToString("F1"));
    }
}
