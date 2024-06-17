using UnityEngine;

public class UISwitch : MonoBehaviour
{
    public GameObject userInfoWindow;
    public GameObject userInvenWindow;
    public GameObject skillWindow;
    public GameObject gachaWindow;
    public GameObject cashStoreWindow;
    public void OnUserInfoWindow()
    {
        userInfoWindow.SetActive(true);
    }
    public void OffUserInfoWindow()
    {
        userInfoWindow.SetActive(false);
    }


    public void OnInvenWindow()
    {
        userInvenWindow.SetActive(true);
    }
    public void OffInvenWindow()
    {
        userInvenWindow.SetActive(false);
    }



    public void OnSkillWindow()
    {
        skillWindow.SetActive(true);
    }
    public void OffSkillWindow()
    {
        skillWindow.SetActive(false);
    }



    public void OnGachaWindow()
    {
        gachaWindow.SetActive(true);
    }
    public void OffGachaWindow()
    {
        gachaWindow.SetActive(false);
    }



    public void OnCashStoreWindow()
    {
        cashStoreWindow.SetActive(true);
    }
    public void OffCashStoreWindow()
    {
        cashStoreWindow.SetActive(false);
    }
}
