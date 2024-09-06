using Entities;
using Managers;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour, IEntityNotify
{
    public Animator anim;
    public Rigidbody rb;
    public AnimatorStateInfo currentBaseState;

    public Entity entity;

    public Vector3 position;
    public Vector3 direction;
    Quaternion rotation;

    public Vector3 lastPosition;
    //public Vector3 lastDirection;
    Quaternion lastRotation;

    public float speed;
    public float animSpeed = 1.5f;
    public float jumpPower = 3.0f;

    public bool isPlayer = false;

    void Start()
    {
        if (entity != null)
        {
            this.UpdateTransform();
        }
        if(!isPlayer)
            rb.useGravity = false;
    }

    void UpdateTransform()
    {
        this.position = GameObjectTool.LogicToWorld(entity.position);
        this.direction = GameObjectTool.LogicToWorld(entity.direction);

        this.rb.MovePosition(this.position);
        this.transform.forward = this.direction;
        this.lastPosition = this.position;
        this.lastRotation = this.rotation;
    }

    private void FixedUpdate()
    {
        if(entity == null) 
            return;
        this.entity.OnUpdate(Time.fixedDeltaTime);
        if (!this.isPlayer)
            this.UpdateTransform();
    }

    public void OnEntityEvent(EntityEvent e)
    {
        switch (e)
        {
            case EntityEvent.None:
                break;
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
        }
    }

    private void OnDestroy()
    {
        if (entity != null)
        {
            Debug.LogFormat("{0} OnDestroy:[id:{1} pos:{2}]", this.name, this.entity.entityId, this.entity.position);
        }
    }

    public void OnEntityRemoved()
    {
        Debug.LogFormat("{0} OnEntityRemoved:[id:{1}]", this.name, this.entity.entityId);
    }

    public void OnEntityChanged(NEntity entity)
    {
        Debug.LogFormat("{0} OnEntityChanged:[id:{1}]", this.name, this.entity.entityId);
    }
}
