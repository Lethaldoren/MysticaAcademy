using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;


public class VoiceTest : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer; // this is what reconizes voice commands and activates the functions

    private Dictionary<string, Action> actions = new Dictionary<string, Action>(); // this is the dictionary that holds all the actions



    // AUSHTONS TEST BUILSHIT
    public Image MagicCircle;
    public bool IsCastingSpell;

    public GameObject PotatoPrefab;
    
    
    // Start is called before the first frame update
    void Start()
    {
        actions.Add("System Call", StartSpell); // you can add actions to the dictionary by using .Add("The words you want to be said to activate", Name of the function)
        actions.Add("Summon Potato", SummonPotato);
        actions.Add("Summon Potato 10", SummonPotato10);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray()); // this adds all actions in the dictionary to the voice commands the keywordRecognizer will listen for
        keywordRecognizer.OnPhraseRecognized += PhraseRecognized;
        keywordRecognizer.Start(); // you can turn the keyword recongnizer on or off

        MagicCircle.gameObject.SetActive(false);

    }


    private void PhraseRecognized (PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsCastingSpell)
        {
            if (MagicCircle.color.a < 255)
            {
                MagicCircle.color = new Color(255, 255, 255, MagicCircle.color.a + (2 * Time.deltaTime)); 
            }
            MagicCircle.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime);

        }
    }

    void StartSpell()
    {
        MagicCircle.gameObject.SetActive(true);
        IsCastingSpell = true;
    }

    void SummonPotato()
    {
        if (IsCastingSpell)
        {
            GameObject Potato = Instantiate(PotatoPrefab, MagicCircle.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
            Potato.GetComponent<Rigidbody>().AddForce(Vector3.up * 10);
            
        }
    }

    void SummonPotato10()
    {
        if (IsCastingSpell)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(PotatoPrefab, MagicCircle.transform.position + new Vector3(0, 3, 0) + UnityEngine.Random.insideUnitSphere * 5, Quaternion.identity);
            }
        }
    }
}
