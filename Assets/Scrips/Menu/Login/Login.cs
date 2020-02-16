using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField login;

    public InputField password;

    public void LoginUser()
    {
        using (SqlConnection dbConnection = new SqlConnection(DbHelper.ConnectionString))
        {
            try
            {
                Text foundErrorObject = BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();
                if (!login.text.Any() || !password.text.Any())
                {
                    BaseHelper.ShowMessageError("Password or username not entered", foundErrorObject);
                    Debug.LogWarning("Password or username not entered");
                    return;
                }

                dbConnection.Open();
                string query = "SELECT PasswordHash, Name FROM Users WHERE Login = @loginUser;";

                SqlCommand command = new SqlCommand(query, dbConnection);

                command.Parameters.Add("@loginUser", SqlDbType.NVarChar).Value = login.text;

                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        BaseHelper.ShowMessageError("User with this login and password does not exist", foundErrorObject);
                        Debug.LogWarning("User with this login not found!");
                        dbConnection.Close();
                        return;
                    }

                    while (reader.Read())
                    {
                        var passwordDbHash = reader.GetString(0);

                        var inputHash = AuthorizationHelper.HashPassword(password.text);

                        if (passwordDbHash == null || inputHash != passwordDbHash)
                        {
                            BaseHelper.ShowMessageError("User with this login and password does not exist", foundErrorObject);
                            Debug.LogWarning("PasswordDbHash is incorrect");
                        }
                        else
                        {
                            PlayerPrefs.DeleteAll();
                            PlayerPrefs.SetString("PlayerName", reader.GetString(1));

                            SceneManager.LoadScene("Menu");
                            ResetFields();
                        }
                    }
                }
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                Text foundErrorObject = BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();
                BaseHelper.ShowMessageError($"{BaseConstants.Messages.SomethingWentWrongMessage}, try later :(", foundErrorObject);
                Debug.LogWarning(ex.ToString());
                ResetFields();
                dbConnection.Close();
            }
        }
    }

    private void ResetFields()
    {
        login.text = string.Empty;
        password.text = string.Empty;
    }
}
