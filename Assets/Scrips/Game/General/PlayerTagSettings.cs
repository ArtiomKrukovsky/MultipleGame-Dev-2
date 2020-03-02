using UnityEngine;

namespace Assets.Scrips.Game.General
{
    public class PlayerTagSettings : MonoBehaviour
    {
        public void Start()
        {
            SetPlayerTag();
        }

        public void SetPlayerTag()
        {
            this.tag = "Player1";
        }
    }
}
