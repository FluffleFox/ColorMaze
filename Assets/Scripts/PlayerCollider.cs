using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Comunication Call;
    public Texture2D Map;
    public Transform Target;

    AudioSource Source;
    public AudioClip[] BounceSounds;

    Rigidbody2D RB;

    int LastCollisionDir = -1;
    Vector3 LastPos;
    Vector3 DeltaPos;

    private void Start()
    {
        Source = GetComponent<AudioSource>();
        RB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        DeltaPos = transform.position - LastPos;
        if (DeltaPos != Vector3.zero)
        {
            LastPos = transform.position;
            Debug.DrawLine(transform.position, transform.position + DeltaPos * 50.0f, Color.green);
        }

        Vector3 Translation = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.deltaTime;

        RB.MovePosition(transform.position + Translation);

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int k = collision.contacts.Length - 1;
        Vector2 To = collision.contacts[k].point - new Vector2(transform.position.x, transform.position.y);
        Vector2 From = new Vector2(DeltaPos.x, DeltaPos.y).normalized;

        if (Vector2.Angle(From, To) < 60.0f)
        {
            int CurrentDir = -1;
            if (Mathf.Abs(collision.contacts[k].point.x - transform.position.x) > Mathf.Abs(collision.contacts[k].point.y - transform.position.y))
            {
                if (collision.contacts[k].point.x - transform.position.x < 0.0f) { CurrentDir = 0; }
                else { CurrentDir = 1; }
            }
            else
            {
                if (collision.contacts[k].point.y - transform.position.y < 0.0f) { CurrentDir = 2; }
                else { CurrentDir = 3; }
            }
            if (CurrentDir != LastCollisionDir)
            {
                Call.Show(Camera.main.WorldToScreenPoint(collision.contacts[k].point));
                Source.PlayOneShot(BounceSounds[Random.Range(0, BounceSounds.Length)]);
                LastCollisionDir = CurrentDir;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        int k = collision.contacts.Length - 1;
        Vector2 To = collision.contacts[k].point - new Vector2(transform.position.x, transform.position.y);
        Vector2 From = new Vector2(DeltaPos.x, DeltaPos.y).normalized;

        if (Vector2.Angle(From, To) < 60.0f)
        {
            int CurrentDir = -1;
            if (Mathf.Abs(collision.contacts[k].point.x - transform.position.x) > Mathf.Abs(collision.contacts[k].point.y - transform.position.y))
            {
                if (collision.contacts[k].point.x - transform.position.x < 0.0f) { CurrentDir = 0; }
                else { CurrentDir = 1; }
            }
            else
            {
                if (collision.contacts[k].point.y - transform.position.y < 0.0f) { CurrentDir = 2; }
                else { CurrentDir = 3; }
            }
            if (CurrentDir != LastCollisionDir)
            {
                Call.Show(Camera.main.WorldToScreenPoint(collision.contacts[k].point));
                Source.PlayOneShot(BounceSounds[Random.Range(0, BounceSounds.Length)]);
                LastCollisionDir = CurrentDir;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        LastCollisionDir = -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Call.EndGame(Camera.main.WorldToScreenPoint(transform.position));
    }
}
