using Unity.VisualScripting;
using UnityEngine;

public class CompositeTestDetector : MonoBehaviour
{
    public float score = 100;
    public float reductor = 5;

    public float cooldown;
    public float cdtimer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Obs") && cdtimer <= 0)
            {
                Debug.Log("Kena" + collision.gameObject.name);
                score -= reductor;
                cdtimer = cooldown;
            }

            if (collision.CompareTag("Finish"))
            {
                // apply clear sequence
                Debug.Log("Cleared");
            }

            return;
        }
    }

    private void Update()
    {
        if(cdtimer > 0)
        {
            cdtimer -= Time.deltaTime;
        }
        else
        {
            cdtimer = 0;
        }
        
    }
}
