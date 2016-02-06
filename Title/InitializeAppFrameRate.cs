using UnityEngine;

namespace GGJ.RuleSelect
{
    /// <summary>
    /// 初期化時にフレームレートを固定化させる
    /// </summary>
    public class InitializeAppFrameRate : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;

            Destroy(this.gameObject);
        }
    }
}

