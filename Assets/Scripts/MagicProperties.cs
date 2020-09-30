using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProperties : MonoBehaviour
{
    [Header("Magic Information")]
    public string magicName;
    public string magicDescription;
    public int magicCost;
    public int magicDamage;
    public Sprite magicSprite;

    [Header("Magic Effects Properties")]
    public RuntimeAnimatorController magicAnimation;
    public float effectLength;
    public int soundEffect;

    [Header("Magic Buff/Debuff Properties")]
    public string buffEffect;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySFX(soundEffect);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, effectLength);
    }
}
