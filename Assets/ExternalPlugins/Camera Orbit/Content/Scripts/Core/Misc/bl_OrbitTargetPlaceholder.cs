using UnityEngine;

namespace Lovatto.OrbitCamera
{
    public class bl_OrbitTargetPlaceholder : MonoBehaviour
    {
        [SerializeField] private Transform m_target = null;
        public Transform Target
        {
            get => m_target;
            set => m_target = value;
        }

        public Vector3 targetOffset;
        public float distance = 5;
        public float angle = 0;
        public float tilt = 0;
        public bool showGizmos = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public bool GetCameraLocation(out Vector3 position, out Quaternion rotation)
        {
            if(m_target == null)
            {
                position = Vector3.zero;
                rotation = Quaternion.identity;
                return false;
            }

            Vector3 targetPos = m_target.position + targetOffset;
            Vector3 ZoomVector = new Vector3(0f, 0f, -distance);
            rotation = Quaternion.AngleAxis(angle, Vector3.up);
            rotation *= Quaternion.AngleAxis(tilt, Vector3.right);
            position = (rotation * ZoomVector) + targetPos;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        void OnDrawGizmosSelected()
        {
            if (!showGizmos) return;
            if (m_target == null) return;

            Vector3 targetPos = m_target.position + targetOffset;
            Quaternion cameraRotation;
            Vector3 cameraPosition;
            GetCameraLocation(out cameraPosition, out cameraRotation);

            Gizmos.color = Color.green;
            if (Target != null)
            {
               
                Gizmos.DrawLine(cameraPosition, targetPos);
                Gizmos.matrix = Matrix4x4.TRS(targetPos, cameraRotation, new Vector3(1f, 0, 1f));
                Gizmos.DrawWireSphere(Vector3.zero, distance);
                Gizmos.matrix = Matrix4x4.identity;
            }
            Gizmos.matrix = Matrix4x4.TRS(cameraPosition, cameraRotation, new Vector3(1f, 1, 1f));
            Gizmos.DrawCube(new Vector3(0, 0, 0.25f), new Vector3(0.2f, 0.2f, 0.5f));
            Gizmos.DrawCube(Vector3.zero, Vector3.one * 0.5f);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}