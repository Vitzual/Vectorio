using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorHandler : MonoBehaviour
{
    // Active instance
    public static CollectorHandler active;

    // Class lists
    public List<DefaultCollector> collectors;

    public void Awake() { active = this; }

    public void Start()
    {
        Events.active.onCollectorPlaced += AddCollector;
    }
    
    public void Update()
    {
        if (Settings.paused) return;

        for(int i = 0; i < collectors.Count; i++)
        {
            if (collectors[i] != null)
            {
                collectors[i].cooldown -= Time.deltaTime;
                if (collectors[i].cooldown < 0f) collectors[i].AddResources();
            }
            else
            {
                collectors.RemoveAt(i);
                i--;
            }
        }
    }

    public void AddCollector(DefaultCollector collector)
    {
        collectors.Add(collector);
    }
}
