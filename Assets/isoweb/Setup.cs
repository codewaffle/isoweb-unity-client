using UnityEngine;

public class Setup : MonoBehaviour {
	void Start ()
	{
        EntityManager.Init();
	    gameObject.AddComponent<Client>();
	}
}
