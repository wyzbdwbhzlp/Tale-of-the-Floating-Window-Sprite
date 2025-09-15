using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpiritMoveState : SpiritState
{
    Spirit spirit;
    private int randomDirX;
    private Camera mainCamera;
    public SpiritMoveState(Spirit _spiritBase, SpiritStateMachine _stateMachine, string _animBoolName) : base(_spiritBase, _stateMachine, _animBoolName)
    {
        spirit = _spiritBase;
        mainCamera = Camera.main;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer=spirit.moveTime;
        randomDirX = UnityEngine.Random.value < 0.5f ? -1 : 1;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        Vector2 boundaryCheckDir = new Vector2(randomDirX, 0);
        RaycastHit2D boundaryHit = Physics2D.Raycast(spirit.transform.position, boundaryCheckDir, 0.5f, LayerMask.GetMask("Boundary"));
        if (randomDirX != 0)
        {
            

            RaycastHit2D hit = Physics2D.Raycast(spirit.transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
            
                Vector2 normal = hit.normal;
                Vector2 tangent = new Vector2(normal.y, -normal.x).normalized;

             
                Vector2 velocity = tangent * (randomDirX * spirit.moveSpeed);
                spirit.Setvelocity(velocity.x, velocity.y);
            }
            else
            {
               
                spirit.Setvelocity(randomDirX * spirit.moveSpeed, spirit.rb.linearVelocity.y);
            }
        }
        AlignToGroundNormal2D();

        
       

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(spirit.idleState);
        }
        if (boundaryHit.collider != null)
        {
           
            spirit.ZeroVelocity();
            stateMachine.ChangeState(spirit.idleState);
            return;
        }
    }
    private void AlignToGroundNormal2D()
    {
        RaycastHit2D hit = Physics2D.Raycast(spirit.transform.position, Vector2.down, 2f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            float angle = Vector2.SignedAngle(Vector2.up, hit.normal);
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            spirit.transform.rotation = Quaternion.Lerp(spirit.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    private void ClampToScreen()
    {
        Vector3 pos = spirit.transform.position;
        Vector3 viewPos = mainCamera.WorldToViewportPoint(pos);

        
        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y);

        spirit.transform.position = mainCamera.ViewportToWorldPoint(viewPos);
    }
}
