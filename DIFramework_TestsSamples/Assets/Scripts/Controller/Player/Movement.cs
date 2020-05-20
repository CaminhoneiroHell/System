using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement{
    public float Speed { get; set; }
    
    public Movement(float speed)
    {
        Speed = speed;       
    }

    public Vector3 Calculate(float horizontalInput, float deltaTime)
    {
        float movement = horizontalInput * Speed * deltaTime;
        return new Vector3(movement, 0, 0);
    }
}
