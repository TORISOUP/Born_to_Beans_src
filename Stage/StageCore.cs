using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Stage
{
    /// <summary>
    /// ステージデータ
    /// </summary>
    public class StageCore : MonoBehaviour
    {
        /// <summary>
        /// プレイヤー出現位置リスト
        /// </summary>
        [SerializeField]
        private List<Transform> StageSpawnPosition;
        
        /// <summary>
        /// アイテム出現位置リスト
        /// </summary>
        [SerializeField]
        private List<Transform> ItemSpawnPosition;
        
        /// <summary>
        /// カメラ設定位置
        /// </summary>
        [SerializeField]
        private Transform CameraPosition;
        
        [SerializeField]
        private Vector3 StageCamera;
        
        void Start()
        {
            foreach( var spown in StageSpawnPosition)
            {
                spown.gameObject.SetActive(false);
            }
            
            foreach( var spown in ItemSpawnPosition)
            {
                spown.gameObject.SetActive(false);
            }
            
            CameraPosition.gameObject.SetActive(false);
        }
        
        //ランダムで生成位置を作る
        public Transform[] GetRandomSpownPosition(int count)
        {
            int max = StageSpawnPosition.Count;
            
            if( max < count)
            {
                return null;
            }
            
            List<int> _createdId = new List<int>();
            Transform[] createPosition = new Transform[count];
            for(int i = 0 ; i < count; ++i)
            {
                while(true)
                {
                    int RandomId = Random.Range( 0, max);
                    if( _createdId.Exists( x => x == RandomId) )
                        continue;
                        _createdId.Add( RandomId);
                    createPosition[i] = StageSpawnPosition[RandomId];
                    break;
                }   
            }
            
            return createPosition;
        }
        
        public Transform GetCameraPosition()
        {
            return CameraPosition;
        }
        
        //ランダムでアイテムの生成位置を返却する
        public List<Transform> GetRandomItemSpownPosition()
        {
            return ItemSpawnPosition;
        }
        
        public Vector3 GetStageCameraAngle()
        {
            return StageCamera;
        }
    }
}