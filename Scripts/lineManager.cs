using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class lineManager : MonoBehaviour
{
    public LineRenderer line;
    public Transform pos1;
    public Transform pos2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, pos1.localPosition);
        line.SetPosition(1, pos2.localPosition);
        
    }
}
