using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private const float ORTOGRAFIC_HORIZINTAL_SIZE = 16f;
    private const float LEFT_END_OF_LEVEL = -16f;
    private const float RIGHT_END_OF_LEVEL = 48f;
    private const float TRESHHOLD = 10f;

    [SerializeField] GameObject objectToFollow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldMove())
            MoveCamera();
    }

    private bool ShouldMove()
    {
        return !EndOfLevelInSight() && CameraPassedTheHorizontalTreshhold();
    }

    private bool EndOfLevelInSight()
    {
        bool leftEndVisible = this.transform.position.x - ORTOGRAFIC_HORIZINTAL_SIZE < LEFT_END_OF_LEVEL;
        bool rightEndVisible = this.transform.position.x + ORTOGRAFIC_HORIZINTAL_SIZE > RIGHT_END_OF_LEVEL;

        if (MovementAmount() < 0)
            return leftEndVisible;
        else if (MovementAmount() > 0)
            return rightEndVisible;
        else
            return true;
    }

    private bool CameraPassedTheHorizontalTreshhold()
    {
        return Mathf.Abs(this.transform.position.x - this.objectToFollow.transform.position.x) > TRESHHOLD;
    }

    private void MoveCamera()
    {
        this.transform.position = new Vector3(
            x: this.transform.position.x + this.MovementAmount(),
            y: this.transform.position.y,
            z: this.transform.position.z
        ); ;
    }

    private float MovementAmount()
    {
        float sign = Mathf.Sign(this.objectToFollow.transform.position.x - this.transform.position.x);

        return (Mathf.Abs(this.transform.position.x - this.objectToFollow.transform.position.x) - TRESHHOLD) * sign;
    }
}
