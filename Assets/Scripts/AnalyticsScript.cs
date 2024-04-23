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

    private string deathAnalyticsFormUrl = "https://docs.google.com/forms/d/e/1FAIpQLScD2tX23JXG1S9bpB11BTljf9RoawvyfuWRFIFEv9M7Na7bCA/formResponse";
    private string entryIdForPlayerId2 = "entry.732915141";
    private string entryIdForLevelNumber2 = "entry.411012388"; 
    private string entryIdForDeathCause = "entry.2113584476"; // Replace XXXX with your actual field ID for death cause

    private string formUrl3 = "https://docs.google.com/forms/d/e/1FAIpQLSeXQ6XqAI6j__F7MgAJjufODrq5fBy0x4na_805CsEvByr2Ig/formResponse";
    private string entryIdForPlayerId3 = "entry.320752607";
    private string entryIdForLevelNumber3 = "entry.621891126";
    private string entryIdForClonesUsed3 = "entry.1033664906";

    private string formUrl4 = "https://docs.google.com/forms/d/e/1FAIpQLSeetdHOHGJUJ6mBRsCElN6H7dOy8KtSfzFHeKFX002oK2kw5A/formResponse";
    private string entryIdForLevelNumber4 = "entry.1999521488";
    private string entryIdForpos = "entry.105677257";

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

    public void TrackDeathAnalytics(string playerId, int levelNumber, string causeOfDeath)
    {
        StartCoroutine(SendDeathAnalyticsToGoogleForm(playerId, levelNumber, causeOfDeath));
    }

    public void TrackCloneAnalytics(string playerId, int levelNumber, int clonesUsed)
    {
        StartCoroutine(SendCloneAnalyticsToGoogleForm(playerId, levelNumber, clonesUsed));
    }

    public void RecordTimeFreezePosition(int levelNumber, string position)
    {
        StartCoroutine(SendPositionToGoogleForm(levelNumber,position));
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
           private IEnumerator SendDeathAnalyticsToGoogleForm(string playerId, int levelNumber, string causeOfDeath)
        {
            WWWForm form = new WWWForm();
            form.AddField(entryIdForPlayerId2, playerId); // Reuse player ID field ID
            form.AddField(entryIdForLevelNumber2, levelNumber.ToString()); // Reuse level number field ID
            form.AddField(entryIdForDeathCause, causeOfDeath); // New field for cause of death

            using (UnityWebRequest www = UnityWebRequest.Post(deathAnalyticsFormUrl, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Death analytics form submit error: " + www.error);
                }
                else
                {
                    Debug.Log("Death analytics form submitted successfully.");
                }
            }
        }

    public IEnumerator SendCloneAnalyticsToGoogleForm(string playerId, int levelNumber, int clonesUsed)
    {
        WWWForm form = new WWWForm();
        form.AddField(entryIdForPlayerId3, playerId);
        form.AddField(entryIdForLevelNumber3, levelNumber.ToString());
        form.AddField(entryIdForClonesUsed3, clonesUsed.ToString()); // Add clones used to form

        using (UnityWebRequest www = UnityWebRequest.Post(formUrl3, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Level completion form submit error: " + www.error);
            }
            else
            {
                Debug.Log("Level completion form submitted successfully.");
            }
        }
    }

    public IEnumerator SendPositionToGoogleForm(int levelNumber, string position)
    {
        WWWForm form = new WWWForm();
        form.AddField(entryIdForLevelNumber4, levelNumber.ToString());
        form.AddField(entryIdForpos, position.ToString()); // Add clones used to form

        using (UnityWebRequest www = UnityWebRequest.Post(formUrl4, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("form submit error: " + www.error);
            }
            else
            {
                Debug.Log("form submitted successfully.");
            }
        }
    }
}




