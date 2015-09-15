using UnityEngine;
using System.Collections;
using GameEnums;
using BoxClasses;

public class PlayerDetectorJumpBox : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        
        if (!GlobalOptions.Player.GetComponent<PlayerController>().isMoving())
        {
            BaseBox box = this.transform.parent.gameObject.GetComponent<BaseBox>();
            if (!box)
                return;

            box.UserStay();
        }

    }
}
