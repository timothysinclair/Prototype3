using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinFloat : MonoBehaviour
{
    public float amp = 1.0f;
    public float period = 1.0f;

    private float startingY;

    // Start is called before the first frame update
    void Start()
    {
        startingY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, startingY + ((float)Mathf.Sin(Time.time * period) * amp), this.transform.position.z);
    }
}
