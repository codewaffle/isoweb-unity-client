using UnityEngine;

namespace isoweb
{
    public class TrackingCamera : MonoBehaviour {
        private GameObject _target;
	
        void Update () {
            if (_target != null)
            {
                transform.position = Vector3.Lerp(transform.position, _target.transform.position, 0.3f) - new Vector3(0,0,10f);
            }
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }
    }
}
