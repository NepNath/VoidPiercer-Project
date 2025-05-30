using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class LoginRegister : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text statusText;

    private string apiUrl = "https://db.ninolbt.com";

    public void OnLoginClick()
    {
        StartCoroutine(SendRequest("/login"));
    }

    public void OnRegisterClick()
    {
        StartCoroutine(SendRequest("/register"));
    }

    IEnumerator SendRequest(string endpoint)
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            statusText.text = "Remplis tous les champs.";
            yield break;
        }

        var json = JsonUtility.ToJson(new Credentials { username = username, password = password });

        UnityWebRequest req = new UnityWebRequest(apiUrl + endpoint, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            statusText.text = $"Succès : {req.downloadHandler.text}";
            Debug.Log("Réponse : " + req.downloadHandler.text);
        }
        else
        {
            statusText.text = $"Erreur : {req.responseCode} - {req.downloadHandler.text}";
            Debug.LogError("Erreur : " + req.error);
        }
    }

    [System.Serializable]
    public class Credentials
    {
        public string username;
        public string password;
    }
}
