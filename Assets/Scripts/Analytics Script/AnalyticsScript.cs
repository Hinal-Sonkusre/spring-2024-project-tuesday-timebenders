using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AnalyticsScript : MonoBehaviour
{
   // private string startLevelUrlTemplate = "https://docs.google.com/forms/d/e/1FAIpQLSc8dIEgJahupWKWKvcNE-wfLr2NSA_sX7M9fSl-338OOJlPHg/viewform?usp=pp_url&entry.1326664972={0}&entry.1267579134={1}";
   // private string completeLevelUrlTemplate = "https://docs.google.com/forms/d/e/1FAIpQLScQZzTQziXwKPE3-oS5bK0U5m4ixPm0Qr3owZizBLhgLQ_i3A/viewform?usp=pp_url&entry.739925269={0}&entry.748710111={1}";

    private string formUrl = "https://docs.google.com/forms/d/e/1FAIpQLSc8dIEgJahupWKWKvcNE-wfLr2NSA_sX7M9fSl-338OOJlPHg/formResponse";
    private string entryIdForPlayerId = "entry.1326664972"; 
    private string entryIdForLevelNumber = "entry.1267579134"; 


    private string formUrl1 = "https://docs.google.com/forms/d/e/1FAIpQLScQZzTQziXwKPE3-oS5bK0U5m4ixPm0Qr3owZizBLhgLQ_i3A/formResponse";
    private string entryIdForPlayerId1 = "entry.739925269"; // Replace XXXXXX with actual entry ID for player ID
    private string entryIdForLevelNumber1 = "entry.748710111"; 
    // Call this method when a level starts
    public void TrackLevelStart(string playerId, int para_level)
    {
        StartCoroutine(SendDataToGoogleForm(playerId, para_level));
    }

    // Call this method when a level is completed
    public void TrackLevelCompletion(string playerId, int level)
    {
        StartCoroutine(SendDataToGoogleForm1(playerId, level));
    }

    private IEnumerator SendDataToGoogleForm(string playerId, int levelNumber)
   {
        WWWForm form = new WWWForm();
        form.AddField(entryIdForPlayerId, playerId);
        form.AddField(entryIdForLevelNumber, levelNumber.ToString());
        Debug.Log(formUrl);

        using (UnityWebRequest www = UnityWebRequest.Post(formUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Form submit error: " + www.error);
            }
            else
            {
                Debug.Log("Form submitted successfully.");
            }
        }
   }

   private IEnumerator SendDataToGoogleForm1(string playerId1, int levelNumber1)
   {
        WWWForm form1 = new WWWForm();
        form1.AddField(entryIdForPlayerId1, playerId1);
        form1.AddField(entryIdForLevelNumber1, levelNumber1.ToString());
        Debug.Log(formUrl1);

        using (UnityWebRequest www = UnityWebRequest.Post(formUrl1, form1))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Form submit error: " + www.error);
            }
            else
            {
                Debug.Log("Form submitted successfully.");
            }
        }
   }
}