using isoweb.Entity;
using UnityEngine;

namespace isoweb
{
    public class Setup : MonoBehaviour
    {
        public Material DefaultMaterial;
        public Canvas UICanvas;
        public GameObject[] SystemPrefabs;

        [SerializeField] private RectTransform[] CanvasPrefabs;

        void Start ()
        {
            Config.DefaultMaterial = DefaultMaterial;
            Global.UICanvas = UICanvas;

            Debug.Log("Instantiating SystemPrefabs");
            foreach (var x in SystemPrefabs)
            {
                Debug.Log("Instantiated: " + x, Instantiate(x));
            }

            foreach (var x in CanvasPrefabs)
            {
                var cp = (RectTransform) Instantiate(x);
                cp.SetParent(UICanvas.transform, false);
            }


            EntityManager.Init();
            gameObject.AddComponent<Client>();
            var mod = Camera.main.gameObject.AddComponent<TrackingCamera>();
        }
    }
}
