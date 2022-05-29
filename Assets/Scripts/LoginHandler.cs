using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Check username and password
    // Switch to Desktop if correct
    public void Login()
    {
        UnityEngine.UI.InputField usernameField, passwordField;
        usernameField = GameObject.Find("Username").GetComponent<UnityEngine.UI.InputField>();
        passwordField = GameObject.Find("Password").GetComponent<UnityEngine.UI.InputField>();
        if (usernameField.text == "root" && passwordField.text == "toor")
        {
            //SceneManager.UnloadSceneAsync(sceneName: "LoginScene");
            SceneManager.LoadScene(sceneName: "DesktopScene", LoadSceneMode.Single);
        }
    }
}
