using UnityEngine;

public static class MoneyManager
{
    public static int GetAmount() =>
        PlayerPrefs.GetInt("money", 5000);

    public static void ChangeAmountBy(int value) =>
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 5000) + value);
}