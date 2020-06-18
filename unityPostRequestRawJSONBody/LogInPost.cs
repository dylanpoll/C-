using System.Collections;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using TMPro;
public class LogInPOST : MonoBehaviour
{
    public TMP_InputField userName;
    public TMP_InputField password;
    public TMP_InputField hostURL; 
    //    public TMP_Text countryTxt;    TMP_Text can be used to desplay data from the json body
    public void fetchData()
    {
        string userEmail = userName.text;       //pulls the user submitted text from text field
        string userPassword = password.text;    //pulls the user submitted text from text field
        LoginCreds credentials = new LoginCreds();
        credentials.email = userEmail;              //sets the values of the serialized class credentials 
        credentials.password = userPassword;        //so that a JSON object can be sent as byte data
        StartCoroutine(LogIn(credentials));
    }
    IEnumerator LogIn(LoginCreds credentials)
    {
        string URL = hostURL.text;  //this is the rest api address pulled from the text field

        string jsonData = JsonUtility.ToJson(credentials);//this turns the class into a json string

        using (UnityWebRequest restAPI = UnityWebRequest.Put(URL+ "/users/logIn", jsonData))
        {
            restAPI.method = UnityWebRequest.kHttpVerbPOST;  //this is declaring that we are actually sending a POST not a PUT, this is a little hack as above we declared it a PUT

            restAPI.SetRequestHeader("content-type","application/json");
            restAPI.SetRequestHeader("Accept","application/json");

            yield return restAPI.SendWebRequest();//sends out the request and waits for the returned content.

            if (restAPI.isNetworkError || restAPI.isHttpError) //checks for errors
            {
                Debug.Log(restAPI.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                if (restAPI.isDone)
                {
                    JSONNode returnedBody = JSON.Parse(System.Text.Encoding.UTF8.GetString(restAPI.downloadHandler.data));
                    if (returnedBody == null)
                    {
                        Debug.Log("failed log in");
                    }
                    else
                    {
                        Debug.Log(returnedBody);//this DISPLAYS the json body to send to the api.
                    }
                }
            }
        }

    }
}

