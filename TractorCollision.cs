using UnityEngine;

public class TractorCollision : MonoBehaviour
{
    public ParticleSystem smoke;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            smoke.Play();
        }
    }
}