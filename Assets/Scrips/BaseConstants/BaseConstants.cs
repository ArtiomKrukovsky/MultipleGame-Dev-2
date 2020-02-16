using UnityEngine;

public class BaseConstants : MonoBehaviour
{
    public static string Network = "Network";

    public static partial class Prefs
    {
        public static string MapName = "MapName";
    }

    public static partial class Messages
    {
        public static string ErrorMessage = "Error message";
        public static string SomethingWentWrongMessage = "Oooppss, something went wrong";
        public static string ManagerNullMessage = "Manager is null";
    }
}
