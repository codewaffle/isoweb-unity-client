using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace isoweb.Entity
{
    public class EntityBehaviour : MonoBehaviour {
        private Queue<Vector4> _updateQueue = new Queue<Vector4>();

        Vector4 _prevUpdate;
        Vector4 _nextUpdate;
        private bool _processing;
        public Entity AttachedEntity { get; set; }

        public void PushPositionUpdate(float stamp, float x, float y, float r)
        {
            _updateQueue.Enqueue(new Vector4(x, y, r, stamp));

            if (isActiveAndEnabled && !_processing)
                StartCoroutine(ProcessQueue());
        }

        void OnEnabled()
        {
            if (_updateQueue.Count > 0 && !_processing)
            {
                StartCoroutine(ProcessQueue());
            }
        }

        IEnumerator ProcessQueue()
        {
            _processing = true;
            while (_updateQueue.Count > 0)
            {
                _prevUpdate = _nextUpdate;
                _nextUpdate = _updateQueue.Dequeue();

                while (Config.Client.ServerTime < _nextUpdate.w)
                {
                    var diff = _nextUpdate.w - _prevUpdate.w;

                    // clamp diff to 150ms.
                    if (diff > 0.150f)
                    {
                        diff = 0.15f;
                        _prevUpdate.w = _nextUpdate.w - diff;
                    }

                    var ratio = (Config.Client.ServerTime - _prevUpdate.w) / diff;
                    var lerp = Vector4.Lerp(_prevUpdate, _nextUpdate, ratio);

                    // re-lerp rotation to avoid huge spins
                    lerp.z = Mathx.LerpBearing(_prevUpdate.z, _nextUpdate.z, ratio);
                    AttachedEntity.SetPosition(lerp.x, lerp.y, lerp.z);

                    // TODO : monitor queue size - we want it to be small (1-2) but probably depends on client latency.
                    yield return new WaitForEndOfFrame();
                }
                AttachedEntity.SetPosition(_nextUpdate.x, _nextUpdate.y, _nextUpdate.z);
            }
            _processing = false;
        }
    }
}
