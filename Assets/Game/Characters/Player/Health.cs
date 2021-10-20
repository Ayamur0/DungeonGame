using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public int health;
    public int maxHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        bool half = health % 2 != 0;
        for (int i = 0; i < maxHealth; i += 2) {
            hearts[i / 2].enabled = i < health;
            hearts[i / 2].sprite = i + 1 == health ? halfHeart : fullHeart;
        }
    }
}
