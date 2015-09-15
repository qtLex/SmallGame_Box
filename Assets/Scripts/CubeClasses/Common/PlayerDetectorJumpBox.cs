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

        GlobalOptions.CurrentBox = this.transform.parent.gameObject;


        if (!GlobalOptions.Player.GetComponent<PlayerController>().isMoving())
        {
            BaseBox box = GlobalOptions.CurrentBox.GetComponent<BaseBox>();
            if (!box)
                return;

            box.UserStay();
        }

    }
}
