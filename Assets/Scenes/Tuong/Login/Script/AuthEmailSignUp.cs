using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AuthEmailSignUp : AuthManager
{
    public void SignUpWithEmail()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Username = signUpUserName.text,
            Email = signUpEmail.text,
            Password = signUpPassword.text,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignUpSucces, OnErrorSignUp);
    }
    private void OnSignUpSucces(RegisterPlayFabUserResult result)
    {
        Debug.Log("Đăng kí người dùng mới thành công!");
        signInPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }
    private void OnErrorSignUp(PlayFabError error)
    {
        Debug.Log("Đăng kí thất bại: " + error.ErrorMessage);
        Debug.LogError("Loại lỗi: " + error.Error.ToString());

    }
}
