using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AuthorizationHelper : MonoBehaviour
{
    public static string HashPassword(string pswd)
    {
        var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(pswd));
        var hashString = new StringBuilder();
        foreach (byte temp in hash)
        {
            hashString.AppendFormat("{0:x2}", temp);
        }

        return hashString.ToString();
    }
}
