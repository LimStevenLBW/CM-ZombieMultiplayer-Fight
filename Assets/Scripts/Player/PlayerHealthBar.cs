using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    private List<GameObject> hearts = new List<GameObject>();
    public int worthPerHeart;
    public void SetHealthBar(float health)
    {
        ClearHealthBar();
        int amountOfHeartsToAdd = (int)health / worthPerHeart;
        for (int i = 0; i < amountOfHeartsToAdd; i++)
        {
            GameObject obj = Instantiate(heartPrefab);
            obj.transform.SetParent(gameObject.transform);
            hearts.Add(obj);
        }
    }


    void ClearHealthBar()
    {
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
