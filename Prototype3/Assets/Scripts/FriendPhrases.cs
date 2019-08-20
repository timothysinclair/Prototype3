using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public enum FriendDone
    {
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX
    }

    public enum FriendInspiration
    {
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX
    }

    public string InspirationMessage(FriendInspiration friendInspiration)
    {
        switch (friendInspiration)
        {
            case FriendInspiration.ONE:
                return "There is always a light at the end of the sea, that light will guide you to your full potential.";
            case FriendInspiration.TWO:
                return "Mountains erupt every so often and burn our surroundings but there is always a option to restore them.";
            case FriendInspiration.THREE:
                return "There are dark times but they can always be illuminated with light.";
            case FriendInspiration.FOUR:
                return "There are more people around you that you are aware off.";
        }
        return "";
    }

    public string DoneMessage(FriendDone friendDone)
    {
        switch (friendDone)
        {
            case FriendDone.ONE:
                return "I shall take some food for the hongi. Catch you soon.";
            case FriendDone.TWO:
                return "Hmmm... I will take some food to the hongi. Good bye for now.";
            case FriendDone.THREE:
                return "Alright I am off to the hongi, talk to you soon.";
            case FriendDone.FOUR:
                return "I can't wait to see you at the feast, let me grab some food.";
        }
        return "";
    }
}
