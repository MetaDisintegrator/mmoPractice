using Entities;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public Rigidbody rb;
    public CharacterState state;

    public Character character;

    public float rotateSpeed = 2.0f;

    public float turnAngle = 10;

    public int speed;

    public EntityController entityController;

    public bool onAir = false;

    void Start()
    {
        state = CharacterState.Idle;
    }

    private void FixedUpdate()
    {
        if (character == null)
            return;

        float v = Input.GetAxis("Vertical");
        if (v > 0.01f)
        {
            if (state != CharacterState.Move)
            {
                state = CharacterState.Move;
                character.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + (this.character.speed + 9.81f) / 100f * GameObjectTool.LogicToWorld(character.direction);
        }
        else if (v < -0.01f)
        {
            if (state != CharacterState.Move)
            {
                state = CharacterState.Move;
                character.MoveBack();
                this.SendEntityEvent(EntityEvent.MoveBack);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + (this.character.speed + 9.81f) / 100f * GameObjectTool.LogicToWorld(character.direction);
        }
        else
        {
            if (state != CharacterState.Idle)
            {
                state = CharacterState.Idle;
                this.rb.velocity = Vector3.zero;
                character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
        }

        if (Input.GetButtonDown("Jump"))
        { 
            this.SendEntityEvent(EntityEvent.Jump);
        }

        float h = Input.GetAxis("Horizontal");
        if (h < -0.01f || h > 0.01f)
        { 
            this.transform.Rotate(0,h*rotateSpeed,0);
            Vector3 dir = GameObjectTool.WorldToLogic(character.direction);
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir,this.transform.forward);

            if ((rot.eulerAngles.y > turnAngle) && (rot.eulerAngles.y < 360 - turnAngle))
            { 
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
                rb.transform.forward = this.transform.forward;
                this.SendEntityEvent(EntityEvent.None);
            }
        }
    }
    private void SendEntityEvent(EntityEvent entityEvent) 
    {
        entityController?.OnEntityEvent(entityEvent);
        MapService.Instance.SendMapEntitySync(entityEvent, character.EntityNetData);
    }

    Vector3 lastPos;
    float lastSync = 0;

    private void LateUpdate()
    {
        if(this.character == null ) 
            return;
        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        lastPos = this.rb.transform.position;

        if ((GameObjectTool.WorldToLogic(this.rb.transform.position) - this.character.position).magnitude > 50)
        { 
            this.character.position = GameObjectTool.WorldToLogic(this.rb.transform.position);
            this.entityController?.OnEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;
    }
}
