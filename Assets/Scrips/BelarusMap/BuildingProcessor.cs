using UnityEngine;

public class BuildingProcessor : MonoBehaviour
{
    public float startRotationX = 0;
    public float startRotationY;

    public GameObject createLobbyPopup;

    private bool _isRotate = false;

    private void OnMouseDown()
    {

        var mapName = transform.name;

        PlayerPrefs.DeleteKey(BaseConstants.Prefs.MapName);
        PlayerPrefs.SetString(BaseConstants.Prefs.MapName, mapName);

        LobbyPopups.ViewCreateLobbyPopup(createLobbyPopup);
    }

    private void OnMouseEnter()
    {
        _isRotate = true;
        transform.localScale = new Vector3(transform.localScale.x + 0.15f, transform.localScale.y + 0.15f, transform.localScale.z + 0.15f);
    }

    private void OnMouseExit()
    {
        transform.rotation = Quaternion.Euler(startRotationX, startRotationY, 0);

        _isRotate = false;
        transform.localScale = new Vector3(transform.localScale.x - 0.15f, transform.localScale.y - 0.15f, transform.localScale.z - 0.15f);
    }

    void Update()
    {
        if (_isRotate)
        {
            transform.Rotate(new Vector3(0, 0.5f, 0));
        }
    }
}
