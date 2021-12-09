using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAPI : MonoBehaviour {
    void TakeDamage(float amount) {
        GetComponent<PlayerStats>().TakeDamage(amount);
    }
}
