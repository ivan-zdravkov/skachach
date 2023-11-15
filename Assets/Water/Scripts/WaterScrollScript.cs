using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScrollScript : MonoBehaviour
{
    [Range(-10, 10)][SerializeField] float backSpeed = 1f;
    [Range(-10, 10)] [SerializeField] float midSpeed = -1.5f;
    [Range(-10, 10)] [SerializeField] float frontSpeed = 2f;

    Renderer rendererComponent;

    float backOffset = 0;
    float midOffset = 0;
    float frontOffset = 0;

    float speedDivider = 200;

    public void Start()
    {
        this.rendererComponent = GetComponent<Renderer>();
    }

    public void Update()
    {
        this.backOffset = Offset(this.backSpeed, this.backOffset, 0);
        this.midOffset = Offset(this.midSpeed, this.midOffset, 1);
        this.frontOffset = Offset(this.frontSpeed, this.frontOffset, 2);
    }

    private float Offset(float speed, float offset, int index)
    {
        offset += (speed / this.speedDivider);

        if (offset > 1.0f)
            offset -= 1.0f;

        Material material = this.rendererComponent.materials[index];

        material.mainTextureOffset = new Vector2(offset, 0);

        return offset;
    }
}
