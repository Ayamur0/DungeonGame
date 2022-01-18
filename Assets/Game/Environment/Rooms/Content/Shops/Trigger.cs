using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Trigger : MonoBehaviour {
    public Action<int> onTriggerEnter;
    public int id;

    void OnTriggerEnter(Collider col) {
        if (onTriggerEnter != null)
            onTriggerEnter(id);
    }

    void OnTriggerExit(Collider col) {
        onTriggerEnter(-1);
    }
}
