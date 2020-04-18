using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField login;

    public InputField password;

    public async void LoginUser()
    {
        using (SqlConnection dbConnection = new SqlConnection(DbHelper.ConnectionString))
        {
            try
            {
                Text foundErrorObject = BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();
                if (!login.text.Any() || !password.text.Any())
                {
                    BaseHelper.ShowMessageError("Пароль или логин не введены", foundErrorObject);
                    Debug.LogWarning("Password or username not entered");
                    SceneLoading.SceneLoadingLogo();
                    return;
                }

                DbDataReader reader = null;
                await Task.Factory.StartNew(() =>
                {
                    dbConnection.Open();
                    string query = "SELECT PasswordHash, Name FROM Users WHERE Login = @loginUser;";

                    SqlCommand command = new SqlCommand(query, dbConnection);

                    command.Parameters.Add("@loginUser", SqlDbType.NVarChar).Value = login.text;

                    reader = command.ExecuteReader();
                });

                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        BaseHelper.ShowMessageError("Пользователь с таким логином и паролем не существует", foundErrorObject);
                        Debug.LogWarning("User with this login not found!");
                        dbConnection.Close();
                        SceneLoading.SceneLoadingLogo();
                        return;
                    }

                    while (reader.Read())
                    {
                        var passwordDbHash = reader.GetString(0);

                        var inputHash = AuthorizationHelper.HashPassword(password.text);

                        if (passwordDbHash == null || inputHash != passwordDbHash)
                        {
                            BaseHelper.ShowMessageError("Пользователь с таким логином и паролем не существует", foundErrorObject);
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

                SceneLoading.SceneLoadingLogo();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                Text foundErrorObject = BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();
                BaseHelper.ShowMessageError($"{BaseConstants.Messages.SomethingWentWrongMessage}, попробуйте позже :(", foundErrorObject);
                Debug.LogWarning(ex.ToString());
                ResetFields();
                SceneLoading.SceneLoadingLogo();
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
