using UnityEngine;

public class BuildingProcessor : MonoBehaviour
{
    public GameObject createLobbyPopup;

    private void OnMouseDown()
    {
        LobbyPopups.ViewCreateLobbyPopup(createLobbyPopup);
    }

    private void OnMouseEnter()
    {

    }
}
