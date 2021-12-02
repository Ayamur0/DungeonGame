using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GameObject> ItemSlots = new List<GameObject>();
    public List<GameObject> AvailableItems = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var slot in ItemSlots)
        {
            var rndIndex = Random.Range(0, AvailableItems.Count - 1);
            var item = this.AvailableItems[rndIndex];
            Instantiate(item, slot.transform);
        }
    }
}