using UnityEngine;
using UnityEngine.SceneManagement;

public class Redirects : MonoBehaviour
{
    public void RedirectToBelarusMap()
    {
        SceneManager.LoadScene("BelarusMap");
    }
}
