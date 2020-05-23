using UnityEngine;

public class BaseConstants : MonoBehaviour
{
    public static string Network = "Network";

    public static string ScoreText = "Quest score";

    public static string Marker = "Marker";

    public static partial class Prefs
    {
        public static string MapName = "MapName";
    }

    public static partial class Messages
    {
        public static string ErrorMessage = "Error message";
        public static string SomethingWentWrongMessage = "Уппсс, что-то пошло не так";
        public static string ManagerNullMessage = "Manager is null";
        public static string QuestNullMessage = "Простите, что-то пошло не так. Попробуйте перезапустить игру";
    }
}
