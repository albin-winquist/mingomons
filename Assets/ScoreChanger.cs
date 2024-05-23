using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreChanger : MonoBehaviour
{
    int score = 0;
    int ammo = 0;
    bool hasNoMana = false;
    string manaChange = "";
    Coroutine blinkCoroutine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "Score: " + score + "\n Ammo: " + ammo + "\n" + manaChange;
    }

    public void GetScore(int num)
    {
        score += num;
    }

    public void GetAmmo(int num)
    {
        ammo = num;
    }

    public void GetMana(bool yup)
    {
        if (hasNoMana != yup)
        {
            hasNoMana = yup;
            if (hasNoMana)
            {
                blinkCoroutine = StartCoroutine(BlinkManaWarning(2.0f));
            }
            else
            {
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }
                manaChange = "";
            }
        }
    }

    IEnumerator BlinkManaWarning(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            manaChange = "Not enough mana!";
            yield return new WaitForSeconds(0.5f);
            manaChange = "";
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 1.0f; 
        }
        manaChange = "";
        hasNoMana = false;
    }
}
