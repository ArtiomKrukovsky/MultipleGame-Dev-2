using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField login;

    public InputField userName;

    public InputField password;

    public async void RegisterUser()
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
                    SceneLoading.SceneLoadingLogo();
                    return;
                }

                if (!IsLoginValid(login.text) || !IsPasswordValid(password.text))
                {
                    BaseHelper.ShowMessageError("This login or password is incorrect", foundErrorObject);
                    Debug.LogWarning("This login or password is incorrect");
                    SceneLoading.SceneLoadingLogo();
                    return;
                }

                string selectLoginQuery = "";
                DbDataReader reader = null;
                await Task.Factory.StartNew(() =>
                {
                    dbConnection.Open();
                    selectLoginQuery = "SELECT Login FROM Users WHERE Login = @loginUserSelect;";
                    SqlCommand selectLoginCommand = new SqlCommand(selectLoginQuery, dbConnection);

                    selectLoginCommand.Parameters.Add("@loginUserSelect", SqlDbType.NVarChar).Value = login.text;
                    reader = selectLoginCommand.ExecuteReader();
                });
            
                using (reader)
                {
                    if (reader.HasRows)
                    {
                        BaseHelper.ShowMessageError("User with this login is exist", foundErrorObject);
                        Debug.LogWarning("User with this login is exist");
                        SceneLoading.SceneLoadingLogo();
                        return;
                    }
                }

                await Task.Factory.StartNew(() =>
                {
                    string query = "Insert into Users (Id, Login, Name, PasswordHash)"
                               + " values (@idUser, @loginUser, @nameUser, @passwordHash) ";

                    SqlCommand command = new SqlCommand(query, dbConnection);

                    command.Parameters.Add("@idUser", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                    command.Parameters.Add("@loginUser", SqlDbType.NVarChar).Value = login.text;
                    command.Parameters.Add("@nameUser", SqlDbType.NVarChar).Value = userName.text;

                    var inputHash = AuthorizationHelper.HashPassword(password.text);
                    command.Parameters.Add("@passwordHash", SqlDbType.NVarChar).Value = inputHash;

                    int rowCount = command.ExecuteNonQuery();
                });

                SceneManager.LoadScene("Login");
                ResetFields();

                SceneLoading.SceneLoadingLogo();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                Text foundErrorObject = BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();
                BaseHelper.ShowMessageError("Oooppss, something went wrong, try later :(", foundErrorObject);
                ResetFields();
                dbConnection.Close();
                Debug.LogWarning(ex.ToString());
            }
        }
    }

    private void ResetFields()
    {
        login.text = string.Empty;
        userName.text = string.Empty;
        password.text = string.Empty;
    }

    private bool IsLoginValid(string userLogin)
    {
        bool isSuccess = true;

        Regex loginRegex = new Regex(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)");
        var result = loginRegex.Match(userLogin).Success;

        isSuccess = result;

        return isSuccess;
    }

    private bool IsPasswordValid(string userPassword)
    {
        bool isSuccess = true;

        Regex loginRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{1,16}$");
        var result = loginRegex.Match(userPassword).Success;

        isSuccess = result;

        return isSuccess;
    }
}
