using UnityEngine;

public class Coin : MonoBehaviour
{
    public float addcoin= 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroy(collision.gameObject);
        collision.GetComponent<PlayerHealth>().CoinAmount(addcoin);
        Destroy(obj: gameObject);
    }
}
