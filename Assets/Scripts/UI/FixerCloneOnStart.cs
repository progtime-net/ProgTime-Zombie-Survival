using UnityEngine;

public class FixerCloneOnStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static bool firstInit = true;
    void Start()
    {

        if (firstInit)
        {
            firstInit = false;
            Instantiate(this);
            Destroy(this);
        }
        else
        {
            firstInit = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
