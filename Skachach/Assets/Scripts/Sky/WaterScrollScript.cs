using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScrollScript : MonoBehaviour
{
    [Range(1, 10)][SerializeField] float speed = 5f;
    Renderer rendererComponent;

    float offset = 0;
    float speedDivider = 20000;

    public void Start()
    {
        this.rendererComponent = GetComponent<Renderer>();    
    }

    public void Update()
    {
        this.offset += (this.speed / this.speedDivider);

        if (this.offset > 1.0f)
            this.offset -= 1.0f;

        this.rendererComponent.material.mainTextureOffset = new Vector2(this.offset, 0);
    }
}
