using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimateAction : Actions
{
    [SerializeField] List<AnimParameter> anims = new List<AnimParameter>();

    [SerializeField] List<Actions> actions = new List<Actions>();

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        for (int i = 0; i < anims.Count; i++)
        {
            anims[i].InitHashID();
        }
    }

    public override void Act()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        int i = 0;

        while(i < anims.Count)
        {
            yield return new WaitForSeconds(anims[i].InvokeDelay);

            animator.SetTrigger(anims[i].HashID);

            i++;

            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(animator.GetNextAnimatorStateInfo(0).length);
        }

        for (int j = 0; j < actions.Count; j++)
        {
            actions[j].Act();
        }
    }
}

[System.Serializable]
public class AnimParameter
{
    [SerializeField] string triggerName;
    [SerializeField] float invokeDelay;

    public float InvokeDelay { get { return invokeDelay; } }

    public int HashID { get; private set; }

    public void InitHashID()
    {
        HashID = Animator.StringToHash(triggerName);
    }
}
