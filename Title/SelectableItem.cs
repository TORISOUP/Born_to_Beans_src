using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class SelectableItem : MonoBehaviour {

    public int id;

    public void Invoke()
    {
        var button = this.GetComponent<Button>();
        button.onClick.Invoke();
    }
}
