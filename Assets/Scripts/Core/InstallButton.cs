using UnityEngine;

public class InstallButton : MonoBehaviour
{
    private const string StoreUrl = "https://play.google.com/store/games";
    
    public void OnClick()
    {
        Application.OpenURL(StoreUrl);
    }
}