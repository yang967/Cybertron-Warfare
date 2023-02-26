using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwayExit : MonoBehaviour
{
    [SerializeField] Highway highway_;


    public void ExitHighway()
    {
        highway_.ExitHighway();
    }
}
