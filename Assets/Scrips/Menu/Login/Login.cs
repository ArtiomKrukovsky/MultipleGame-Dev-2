using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private string _connectionString;

    public InputField login;

    public InputField password;

    public void LoginUser()
    {
        _connectionString = @"Data Source = SQL5041.site4now.net; 
        User Id = DB_A50AD1_broadwood_admin;
        Password = qwe123ZXC.;
        Initial Catalog = DB_A50AD1_broadwood;";

        using (SqlConnection dbConnection = new SqlConnection(_connectionString))
        {
            try
            {
                Text foundErrorObject = FindObjectByTag("Error message");
                if (!login.text.Any() || !password.text.Any())
                {
                    ShowMessageError("Password or username not entered", foundErrorObject);
                    Debug.LogWarning("Password or username not entered");
                    return;
                }

                dbConnection.Open();
                Debug.Log("Connected to database.");
                
                string query = "SELECT PasswordHash, Name FROM Users WHERE Login = @loginUser;";

                SqlCommand command = new SqlCommand(query, dbConnection);

                command.Parameters.Add("@loginUser", SqlDbType.NVarChar).Value = login.text;

                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        ShowMessageError("User with this login and password does not exist", foundErrorObject);
                        Debug.LogWarning("User with this login not found!");
                        dbConnection.Close();
                        return;
                    }

                    while (reader.Read())
                    {
                        // Index = 0 (select 1 parameter in query).
                        var passwordDbHash = reader.GetString(0);

                        //hash
                        var inputHash = HashPassword(password.text);

                        if (passwordDbHash == null || inputHash != passwordDbHash)
                        {
                            ShowMessageError("User with this login and password does not exist", foundErrorObject);
                            Debug.LogWarning("PasswordDbHash is incorrect");
                        }
                        else
                        {
                            PlayerPrefs.DeleteAll();
                            PlayerPrefs.SetString("PlayerName", reader.GetString(1));

                            Debug.Log("Login Confirmed.");
                            SceneManager.LoadScene("Menu");
                            ResetFields();
                        }
                    }
                }
                dbConnection.Close();
                Debug.Log("Connection to database closed.");
            }
            catch (Exception ex)
            {
                Text foundErrorObject = FindObjectByTag("Error message");
                ShowMessageError("Oooppss, something went wrong, try later :(", foundErrorObject);
                Debug.LogWarning(ex.ToString());
                ResetFields();
                dbConnection.Close();
            }
        }
    }

    private string HashPassword(string pswd)
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

    private void ResetFields()
    {
        login.text = string.Empty;
        password.text = string.Empty;
    }

    private Text FindObjectByTag(string nameOfObject)
    {
        return GameObject.FindGameObjectWithTag(nameOfObject).GetComponent<Text>();
    }

    private void ShowMessageError(string message, Text textObject)
    {
        textObject.text = message;
    }
}
