using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AuthEmailSignIn : AuthManager
{
    public void SignInGame()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = signInEmail.text,
            Password = signInPassword.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnSignInSuccess, OnErrorSignIn);
    }
    private void OnSignInSuccess(LoginResult result)
    {
        Debug.Log("Đăng nhập thành công");
        SceneManager.LoadScene(levelIndex);
    }
    private void OnErrorSignIn(PlayFabError error)
    {
        Debug.Log("Đăng nhập thất bại: " + error.ErrorMessage);
    }
}
