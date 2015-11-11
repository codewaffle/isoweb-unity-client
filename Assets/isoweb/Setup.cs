using UnityEngine;

public class Setup : MonoBehaviour
{
    public Material DefaultMaterial;
    void Start ()
    {
        Config.DefaultMaterial = DefaultMaterial;
        EntityManager.Init();
	    gameObject.AddComponent<Client>();
	    var mod = Camera.main.gameObject.AddComponent<TrackingCamera>();
	}
}
