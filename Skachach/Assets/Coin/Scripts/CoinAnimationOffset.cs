using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimationOffset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Animator>().SetFloat("cycleOffset", (Mathf.Floor(this.transform.position.x) % 10) / 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
