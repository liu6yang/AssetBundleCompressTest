using System.Collections;
using UnityEngine;

namespace JJYGame
{
    public class CaptureGemCellDestroy : MonoBehaviour
    {
        private Transform m_Target;
        public void Destroy(Transform target)
        {
            if (target == null)
            {
                return;
            }
            m_Target = target;

            StartCoroutine(BeginAnim());
        }
        IEnumerator BeginAnim()
        {
            if (m_Target == null)
            {
                yield break;
            }
            GameObject go = m_Target.gameObject;
            Vector3[] path = new Vector3[2];
            Vector3 pos = go.transform.position;
            float x = Random.Range(-1.9f, 1.9f);
            float y = Random.Range(0.4f, 0.8f);
            path[0] = new Vector3(pos.x + x / 2, pos.y + y, pos.z);
            pos.y -= 2.0f;
            pos.x += x;

            yield break;
        }

        void End()
        {

        }
    }
}
