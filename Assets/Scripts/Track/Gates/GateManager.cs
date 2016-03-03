using UnityEngine;
using System.Collections;

public class GateManager : MonoBehaviour
{
    public Gate[] Gates;

	void Start ()
    {
        Gates = GetComponentsInChildren<Gate>();
	}
	
	void Update ()
    {
	    
	}


}
