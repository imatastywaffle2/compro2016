using UnityEngine;
using System.Collections;
public class PickupUI : MonoBehaviour {

    public GameObject[] pickupIcon;
    public int activeIndex = -1;

    public void enableIcon(Pickups pickup)
    {
        int index = (int)pickup;
        if (activeIndex != index)
        {
            pickupIcon[index].SetActive(true);
            activeIndex = index;
        }
        else if (activeIndex > -1)
        {
            pickupIcon[index].SetActive(true);
            activeIndex = index;
        }

    }

    public void disableIcon()
    {
        if(activeIndex > -1)
        {
            pickupIcon[activeIndex].SetActive(false);
            activeIndex = -1;
        }
    }
	
}
