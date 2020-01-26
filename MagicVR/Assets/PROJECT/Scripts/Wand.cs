using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


using UnityEngine.Windows.Speech;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class Wand : MonoBehaviour
    {

        public SteamVR_Action_Boolean m_FireAction = null; 
        public Transform m_WandTip = null;


        //private vartiables
        private SteamVR_Behaviour_Pose m_Pose = null;
        private bool CastingSpell;

        private EquipableSpell EquipedSpell; // the currently equiped spell; 
        public List<EquipableSpell> AllSpells = new List<EquipableSpell>();

        public List<String> spellNames = new List<string>();

        private String EquipedSpellName; 

        private KeywordRecognizer keywordRecognizer; // this is what reconizes voice commands and activates the functions

        // Start is called before the first frame update
        void Start()
        {
            //search for every monobehaviour using the equipable spell interface. 
            foreach (MonoBehaviour MB in gameObject.GetComponentsInChildren<MonoBehaviour>()){

                if (MB is EquipableSpell)
                {
                    EquipableSpell equipableSpell = (EquipableSpell)MB;
                    AllSpells.Add(equipableSpell);
                }
            }

            //convert the names of every spell into strings readable by the keyword recognizer
            foreach (EquipableSpell equipableSpell in AllSpells)
            {
                String spellName = StringModifiers.AddSpacesToSentence(equipableSpell.GetType().ToString());
                spellNames.Add(spellName);
            }

            keywordRecognizer = new KeywordRecognizer(spellNames.ToArray()); // this adds all actions in the dictionary to the voice commands the keywordRecognizer will listen for
            keywordRecognizer.OnPhraseRecognized += PhraseRecognized;
            keywordRecognizer.Start(); // you can turn the keyword recongnizer on or off

            m_Pose = GetComponentInParent<SteamVR_Behaviour_Pose>();
        }

        private void PhraseRecognized(PhraseRecognizedEventArgs speech)
        {
            //change the word recongnized by the phrase recongnizer to the name of the monobehavior
            EquipedSpellName = StringModifiers.RemoveWhitespace(speech.text);
            if (!CastingSpell)
            {
                EquipedSpell = AllSpells.Find((x) => x.GetType().ToString() == EquipedSpellName);
                Debug.Log("Spell Equiped! : " + speech.text);
            }

            EquipedSpell.OnEquip();
        }

        // Update is called once per frame
        void Update()
        {
           /*
            if (m_FireAction.GetStateDown(m_Pose.inputSource))
            {
                EquipedSpell.OnTriggerDown();
                CastingSpell = true; 
            }

            if (CastingSpell)
            {
                EquipedSpell.OnTriggerHeld();
            }

            if (m_FireAction.GetStateUp(m_Pose.inputSource))
            {
                EquipedSpell.OnTriggerUp();
                CastingSpell = false; 
            }
*/
        }


    }


}




