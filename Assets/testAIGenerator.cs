using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAIGenerator : MonoBehaviour
{
    public bool generateButton = false;
    public float mode;

    public List<GameObject> ai_bots = new List<GameObject>();

    public void Update()
    {
        if (generateButton)
        {
            generateButton = false;
            
            if (ai_bots != null)
            {
                ai_bots.ForEach(bot => bot.GetComponent<EnemyController>().ReceiveDamage(100f));
                
            }
            Invoke(nameof(testGenerate), 2f);
        }
    }

    public void testGenerate()
    {
        ai_bots = GetComponent<EnemyGenerator>().GenerateEnemies(mode, null);
    }
}
