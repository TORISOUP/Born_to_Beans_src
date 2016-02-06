using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ.Utils
{
    public enum SceneNameEnum
    {
        Title,
        RuleSelect,
        GameMain,
        Result
    }

    public class FadeSceneTransition : SingletonMonoBehaviour<FadeSceneTransition>
    {
        /// <summary>暗転用黒テクスチャ</summary>
        private Texture2D _blackTexture;

        /// <summary>フェード中の透明度</summary>
        private float _fadeAlpha = 0;

        private ReactiveProperty<bool> _isFading = new BoolReactiveProperty(false);

        /// <summary>フェード中かどうか</summary>
        public ReadOnlyReactiveProperty<bool> IsFading
        {
            get { return _isFading.ToReadOnlyReactiveProperty(); }
        }

        public void Awake()
        {
            if (this != Instance)
            {
                Destroy(this);
                return;
            }

            DontDestroyOnLoad(this.gameObject);

            StartCoroutine(CreateBlackTexture());
        }

        IEnumerator CreateBlackTexture()
        {
            yield return new WaitForEndOfFrame();

            //ここで黒テクスチャ作る
            this._blackTexture = new Texture2D(32, 32, TextureFormat.RGB24, false);
            this._blackTexture.ReadPixels(new Rect(0, 0, 32, 32), 0, 0, false);
            this._blackTexture.SetPixel(0, 0, Color.white);
            this._blackTexture.Apply();
        }


        public void OnGUI()
        {
            if (!_isFading.Value) return;
            //透明度を更新して黒テクスチャを描画
            GUI.color = new Color(0, 0, 0, this._fadeAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this._blackTexture);
        }

        /// <summary>
        /// フェードしながらシーン移動
        /// </summary>
        /// <param name="sceneName">シーン名</param>
        /// <param name="interval">フェードインアウトまでの時間</param>
        public void FadeChangeScene(SceneNameEnum sceneName, float interval = 1f)
        {
            // フェード中は無視
            if (_isFading.Value) return;
            StartCoroutine(FadeChangeSceneAsCorutine(sceneName, interval));
        }


        /// <summary>
        /// フェード用コルーチン
        /// </summary>
        private IEnumerator FadeChangeSceneAsCorutine(SceneNameEnum sceneName, float interval)
        {
            // フェード開始
            _isFading.Value = true;

            float time = 0;
            while (time <= interval)
            {
                _fadeAlpha = Mathf.Lerp(0f, 1f, time/interval);
                time += Time.deltaTime;
                yield return 0;
            }

            // シーン読み込み
            SceneManager.LoadScene(sceneName.ToString());

            time = 0;
            while (time <= interval)
            {
                _fadeAlpha = Mathf.Lerp(1f, 0f, time/interval);
                time += Time.deltaTime;
                yield return 0;
            }

            // フェード完了
            _isFading.Value = false;

        }
    }
}
