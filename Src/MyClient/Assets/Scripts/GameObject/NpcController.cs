using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public int npcId;

    Animator animator;
    public SkinnedMeshRenderer render;
    Color originalColor;

    public NpcDefine Define { get; set; }

    bool interacting= false;

    void Start()
    {
        Define = DataManager.Instance.Npcs[npcId];
        animator = GetComponent<Animator>();
        originalColor = render.sharedMaterials[0].color;
        StartCoroutine(RelaxLoop());
    }

    private void OnMouseDown()
    {
        if (!interacting)
        { 
            interacting = true;
            StartCoroutine(Interact());
        }
    }
    IEnumerator Interact()
    {
        yield return Face2Player();
        if (NpcManager.Instance.Interact(npcId))
        {
            animator.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3);
        interacting= false;
    }
    IEnumerator Face2Player()
    { 
        Vector3 dir = (User.Instance.CurrentAvatarObj.transform.position - this.transform.position).normalized;
        dir.y = 0;
        while (System.Math.Abs(Vector3.Angle(this.transform.forward, dir)) > 5)
        {
            transform.forward = Vector3.Slerp(this.transform.forward, dir, Time.deltaTime * 5f);
            yield return null;
        }
        transform.forward = dir;
        yield return null;
    }

    IEnumerator RelaxLoop()
    {
        while (true)
        {
            if (interacting)
                yield return new WaitForSeconds(2);
            else
            {
                yield return new WaitForSeconds(Random.Range(5f, 10f));
                animator.SetTrigger("Relax");
            }
        }
    }
    private void OnMouseEnter()
    {
        if (render.sharedMaterials[0].color != Color.white)
            render.sharedMaterials[0].color = Color.white;
    }
    private void OnMouseOver()
    {
        if (render.sharedMaterials[0].color != Color.white)
            render.sharedMaterials[0].color = Color.white;
    }
    private void OnMouseExit()
    {
        render.sharedMaterials[0].color = originalColor;
    }
}
