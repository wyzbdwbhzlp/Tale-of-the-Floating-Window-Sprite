using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    public int facingFlipDir { get; private set; } = 1;
    protected bool facingRight=true;
    public System.Action onFlipped;

    [Header("Reference")]
    public Transform spriteHolder;
    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float moveTime;
    public SpiritStateMachine stateMachine{ get; private set; }
    public virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        if (spriteHolder == null)
        {
            spriteHolder = transform; // 如果没拖拽，就用自己
        }
    }
    public void ZeroVelocity()
    {

        rb.linearVelocity = new Vector2(0, 0);
    }
    public void Setvelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    public virtual void Flip()
    {
        facingFlipDir = facingFlipDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        //  只翻转子物体，不动父物体
        Vector3 scale = spriteHolder.localScale;
        scale.x *= -1;
        spriteHolder.localScale = scale;

        onFlipped?.Invoke();
    }
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    public virtual void Awake()
    {
        stateMachine=new SpiritStateMachine();
    }
    public virtual void Update()
    {
        
    }

}
