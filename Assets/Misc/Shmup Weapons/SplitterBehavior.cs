using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.Mechanics;

public class SplitterBehavior : MonoBehaviour {

    public int numberOfSplits = 3;  // number of times to split
    public int splitFactor = 2; // split by this each time
    public float sizeReduction = 0.5f; // scale size and health by this
    public string poolTag; // tag name for object pooler

    Health health;
    ObjectPooler objectPooler;
    // Use this for initialization
    void Start()
    {
        health = GetComponent<Health>();
        health.OnHealthDroppedToZero += Split;
        objectPooler = ObjectPooler.Instance;
    }
    

    void Split()
    {
        Debug.Log(name + " splitting into " + splitFactor + ", " + (sizeReduction * 100) + "% smaller versions");
        if (numberOfSplits > 0)
        {
            for (int i = 0; i < splitFactor; i++)
            {
                GameObject split = objectPooler.SpawnFromPool(poolTag, transform.position, Quaternion.identity);
                var sb = split.GetComponent<SplitterBehavior>();

                sb.numberOfSplits = numberOfSplits - 1;
                sb.splitFactor = splitFactor;
                sb.sizeReduction = sizeReduction;

                split.GetComponent<Health>().MaxHealth = Mathf.RoundToInt(health.MaxHealth * sizeReduction);

                split.transform.localScale = transform.localScale * sizeReduction;
            }
            
        }

        gameObject.SetActive(false);
    }
	

}
