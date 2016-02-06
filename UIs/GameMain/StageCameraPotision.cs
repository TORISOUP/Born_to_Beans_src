// #define USE_FOV
#define USE_FOLLOW

using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using GGJ.Player;
using System.Linq;
using GGJ.GameManager;

namespace GGJ.UIs
{
    [RequireComponent(typeof(Camera))]
    public class StageCameraPotision : MonoBehaviour
    {

        [SerializeField]
        private Vector2 m_RotationRange;
        [SerializeField]
        private float m_FollowSpeed = 1;
        [SerializeField]
        private float m_ZoomAmountMultiplier = 2;      // a multiplier for the FOV amount. The default of 2 makes the field of view twice as wide as required to fit the target.

        private Vector3 m_FollowAngles;
        private Quaternion m_OriginalRotation;
        private Vector3 tergetPos = Vector3.zero;
        protected Vector3 m_FollowVelocity;

        private Vector3 m_defaultPosition;

        private float m_FovAdjustVelocity;
        Camera m_Cam = null;

        void Start()
        {
            m_Cam = GetComponent<Camera>();
            m_OriginalRotation = transform.localRotation;

            this.UpdateAsObservable()
                .Where(_ => GameState.Instance.GameStateReactiveProperty.Value == GameStateEnum.Countdown ||
                            GameState.Instance.GameStateReactiveProperty.Value == GameStateEnum.GameUpdate)
                .Select(_ =>
                {
                    var playerPos = PlayerManager.Instance.GetAlivePlayers()
                        .Select(x => x.transform.position);

                    var _x = playerPos.Average(x => x.x);
                    var _y = playerPos.Average(x => x.y);
                    var _z = playerPos.Average(x => x.z);

                    tergetPos = new Vector3(_x, _y, _z);
                    return tergetPos;

                }).DelayFrame(3)
                .Subscribe(target =>
                {
                    var campos = tergetPos + m_defaultPosition;
                    transform.position = Vector3.Lerp(this.transform.position, campos, Time.deltaTime * 5.0f);
             //       transform.LookAt(target - this.transform.position);
                });

        }

        public void SetPosition(Transform transform, Vector3 stageCameraSize)
        {
            this.transform.position = transform.position;
           // this.transform.rotation = transform.rotation;

            m_OriginalRotation = transform.localRotation;
            m_defaultPosition = stageCameraSize;
        }

        void Update()
        {
            return;
            ;
            if (GameState.Instance.GameStateReactiveProperty.Value == GameStateEnum.Countdown ||
                GameState.Instance.GameStateReactiveProperty.Value == GameStateEnum.GameUpdate)
            {
                var plaerPos = PlayerManager.Instance.GetAlivePlayers()
                    .Select(x => x.transform.position);

                var _x = plaerPos.Average(x => x.x);
                var _y = plaerPos.Average(x => x.y);
                var _z = plaerPos.Average(x => x.z);

                tergetPos = new Vector3(_x, _y, _z);
                var camPos = tergetPos + m_defaultPosition;
#if USE_FOLLOW
                //FollowTarget();
                transform.position = Vector3.Lerp(this.transform.position, camPos, Time.deltaTime * 10.0f);
#endif
                m_ZoomAmountMultiplier = tergetPos.magnitude * 0.0001f * 5f;
                // calculate the correct field of view to fit the bounds size at the current distance
                float dist = (tergetPos - camPos).magnitude;
                float requiredFOV = dist * Mathf.Rad2Deg * m_ZoomAmountMultiplier;

#if USE_FOV
                m_Cam.fieldOfView = Mathf.SmoothDamp(m_Cam.fieldOfView, requiredFOV, ref m_FovAdjustVelocity, 1);
#endif

            }
        }

        protected void FollowTarget()
        {
            transform.localRotation = m_OriginalRotation;

            Vector3 localTarget = transform.InverseTransformPoint(tergetPos);
            float yAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            yAngle = Mathf.Clamp(yAngle, -m_RotationRange.y * 0.5f, m_RotationRange.y * 0.5f);
            transform.localRotation = m_OriginalRotation * Quaternion.Euler(0, yAngle, 0);

            localTarget = transform.InverseTransformPoint(tergetPos);
            float xAngle = Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;
            xAngle = Mathf.Clamp(xAngle, -m_RotationRange.x * 0.5f, m_RotationRange.x * 0.5f);
            var targetAngles = new Vector3(m_FollowAngles.x + Mathf.DeltaAngle(m_FollowAngles.x, xAngle),
                                           m_FollowAngles.y + Mathf.DeltaAngle(m_FollowAngles.y, yAngle));

            m_FollowAngles = Vector3.SmoothDamp(m_FollowAngles, targetAngles, ref m_FollowVelocity, m_FollowSpeed);

            transform.localRotation = m_OriginalRotation * Quaternion.Euler(-m_FollowAngles.x, m_FollowAngles.y, 0);
        }
    }


}