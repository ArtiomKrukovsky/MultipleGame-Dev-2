using UnityEngine;

public class SceneLoading : MonoBehaviour
{
    public static void SceneLoadingLogo()
    {
        var loading = GameObject.Find("LoadingBase").transform.Find("Loading").gameObject;

        if (!loading.active)
        {
            loading.SetActive(true);
        }
        else
        {
            loading.SetActive(false);
        }
    }
}
