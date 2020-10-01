using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProperties : MonoBehaviour
{
    [Header("Skill Information")]
    public string skillName;
    public string skillDescription;
    public int skillCost;
    public int skillDamage;
    public Sprite skillSprite;

    [Header("Skill Effects Properties")]
    public RuntimeAnimatorController skillAnimation;
    public float effectLength;
    public int soundEffect;

    [Header("Skill Buff/Debuff Properties")]
    public TimedBuff buffEffect;
    
    [Header("Current/Target User Properties")]
    public GameObject currentUser;
    public GameObject targetUser;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySFX(soundEffect);
        
        // Apply buff
        if (buffEffect != null)
        {
            targetUser.GetComponent<BuffableEntity>().targetUser = targetUser;
            targetUser.GetComponent<BuffableEntity>().AddBuff(buffEffect);
            targetUser.GetComponent<BuffableEntity>().Tick();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, effectLength);
    }
}
