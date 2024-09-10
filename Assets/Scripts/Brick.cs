using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Brick : MonoBehaviour
{
    public Sprite[] States = new Sprite[0];
    public int Points = 100;
    public bool Unbreakable;

    private SpriteRenderer _spriteRenderer;
    private int _health;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ResetBrick();
    }

    public void ResetBrick()
    {
        gameObject.SetActive(true);

        if (!Unbreakable)
        {
            _health = States.Length;
            _spriteRenderer.sprite = States[_health - 1];
        }
    }

    private void Hit()
    {
        if (Unbreakable)
        {
            return;
        }

        _health--;

        if (_health <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _spriteRenderer.sprite = States[_health - 1];
        }

        GameManager.Instance.OnBrickHit(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball")
        {
            Hit();
        }
    }
}
