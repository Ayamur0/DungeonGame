using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public Image[] heartImages;
    public Sprite[] healthSprites;

    private PlayerStats stats;
    private int[] sprites;
    // Start is called before the first frame update
    void Start() {
        stats = this.GetComponent<PlayerStats>();
        sprites = new int[stats.maxHearts];
    }

    // Update is called once per frame
    void Update() {
        UpdateSpecialHearts();
        UpdateHeartContainers();
        UpdateHearts();
    }

    void UpdateHearts() {
        for (int i = 0; i < sprites.Length; i++)
            heartImages[i].sprite = healthSprites[sprites[i]];
    }

    void UpdateHeartContainers() {
        for (int i = 0; i < stats.maxHearts; i++)
            heartImages[i].enabled = i < stats.heartContainers;
    }

    void UpdateSpecialHearts() {
        float lastBlackHeartIndex = Mathf.Ceil(stats.redHearts) + stats.blackHearts;
        float lastSoulHeartIndex = Mathf.Ceil(lastBlackHeartIndex) + stats.soulHearts;
        for (int i = 0; i < sprites.Length; i++) {
            if (i < stats.redHearts) {
                sprites[i] = i + 0.5 < stats.redHearts ? 1 : 2;
            } else if (i < lastBlackHeartIndex)
                sprites[i] = i + 0.5 < lastBlackHeartIndex ? 5 : 6;
            else if (i < lastSoulHeartIndex)
                sprites[i] = i + 0.5 < lastSoulHeartIndex ? 3 : 4;
            else
                sprites[i] = 0;
        }
    }
}
