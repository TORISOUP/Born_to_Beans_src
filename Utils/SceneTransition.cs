using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ.Utils
{


    public class SceneTransition : MonoBehaviour
    {
        public static void ChangeScene(SceneNameEnum scene_enum)
        {
            SceneManager.LoadScene(scene_enum.ToString());
        }
    }
}
