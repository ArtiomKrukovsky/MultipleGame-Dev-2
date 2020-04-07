using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectToRegistration : MonoBehaviour
{
    public void LoadSceneRegistration()
    {
        SceneManager.LoadScene("Registration");
        Debug.Log("Redirect to scene Registration.");
    }

    public void RedirectToLoginScene()
    {
        SceneManager.LoadScene("Login");
        Debug.Log("Redirect to scene Login.");
    }
}
