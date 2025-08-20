using UnityEngine;

public class ZombieCollisionScript : MonoBehaviour
{
    [SerializeField] public string name = "SimpleZombieHead";
    [SerializeField] public int id;
    [SerializeField] public bool isHead = false;
    [SerializeField] public Collider collider;
    [SerializeField] public ZombieController owner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
