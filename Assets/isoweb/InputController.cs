using UnityEngine;
using UnityEngine.EventSystems;

namespace isoweb
{
    public class InputController : MonoBehaviour {
        private bool _dragging;
        private bool _moving;
        private float _nextMove;

        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
            if (Input.GetMouseButtonDown(0))
            {
                if(!EventSystem.current.IsPointerOverGameObject())
                    StartDrag();
            }

            if (_dragging && Input.GetMouseButton(0))
            {
                PerformDrag();
            }

            if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }

        private void PerformDrag()
        {
            if (_moving &&  Time.time > _nextMove)
            {
                var clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Config.Client.SendCmdContextualPosition(clickPos.x, clickPos.y);
                _nextMove = Time.time + 0.05f;
            
            }
        }

        private void StartDrag()
        {
            _dragging = true;

            // TODO : only move if not clicking on something more interesting
            _moving = true;
            _nextMove = 0f;
        }

        void EndDrag()
        {
            _dragging = false;
            _moving = true;
        }
    }
}
