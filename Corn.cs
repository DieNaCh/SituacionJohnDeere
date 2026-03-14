using UnityEngine;

public class Corn : MonoBehaviour
{
    public int points = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManagerScript.instance.AddScore(points);
            Destroy(gameObject);
        }
    }
}