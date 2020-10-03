using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDamageNumber : MonoBehaviour
{
    public Text damageText;

    public float lifetime = 1f;
    public float moveSpeed = 1f;

    public float placementJitter = 2f;

    private Color yellowColor = new Color(0.8f, 0.8f, 0f);
    private Color greenColor = new Color(0f, 0.8f, 0.6f);

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }

    public void SetDamage(int damageAmount)
    {
        // Set to "+" if it's a heal
        if (damageAmount < 0)
        {
            damageText.text = damageAmount.ToString().Replace("-","+");
            damageText.color = greenColor;
        }
        else
        {
            damageText.text = damageAmount.ToString();
        }

        transform.position += new Vector3(Random.Range(-placementJitter*1.5f, placementJitter*1.5f), Random.Range(-placementJitter, placementJitter), 0f);
    }

    public void Miss()
    {
        damageText.text = "MISS";
        damageText.color = yellowColor;
        transform.position += new Vector3(Random.Range(-placementJitter * 1.5f, placementJitter * 1.5f), Random.Range(-placementJitter, placementJitter), 0f);
    }
}
