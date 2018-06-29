using UnityEngine;
using Kpable.Mechanics;

[RequireComponent(typeof(Health))]
public class DieOnHealthDepletion : MonoBehaviour {
    Health health;
	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        health.OnHealthDroppedToZero += HandleHealthDroppedToZero;
	}

    private void HandleHealthDroppedToZero()
    {
        gameObject.SetActive(false);
    }
}
