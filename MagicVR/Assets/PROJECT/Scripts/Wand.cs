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

        // the currently equiped spell;
        [SerializeField]
        private EquipableSpell EquipedSpell;
        private EquipableSpell[] AllSpells;

        List<string> spellWords = new List<string>();

        private string EquipedSpellName;

        // this is what reconizes voice commands and activates the functions
        private KeywordRecognizer keywordRecognizer; 

        // -------------------------------------------------------

        void Start()
        {
            //search for every monobehaviour using the equipable spell interface. 
            //foreach (MonoBehaviour MB in gameObject.GetComponentsInChildren<MonoBehaviour>()){

            //    if (MB is EquipableSpell)
            //    {
            //        //EquipableSpell equipableSpell = (EquipableSpell)MB;
            //        //AllSpells.Add(equipableSpell);
            //    }
            //}

            //convert the names of every spell into strings readable by the keyword recognizer
            AllSpells = SpellManager.Instance.GetComponents<EquipableSpell>();
            foreach (EquipableSpell spell in AllSpells)
            {
                spellWords.Add(spell.magicWords);
            }

            keywordRecognizer = new KeywordRecognizer(spellWords.ToArray()); // this adds all actions in the dictionary to the voice commands the keywordRecognizer will listen for
            keywordRecognizer.OnPhraseRecognized += PhraseRecognized;
            keywordRecognizer.Start(); // you can turn the keyword recongnizer on or off

            m_Pose = GetComponentInParent<SteamVR_Behaviour_Pose>();
        }

        // ------------------------------------------------------------

        private void PhraseRecognized(PhraseRecognizedEventArgs speech)
        {
            //change the word recongnized by the phrase recongnizer to the name of the monobehavior
            //EquipedSpellName = StringModifiers.RemoveWhitespace(speech.text);
            if (!CastingSpell)
            {
                EquipedSpell = AllSpells.First(s => s.magicWords == speech.text);
                Debug.Log("Spell Equiped! : " + speech.text);
            }
            EquipedSpell = Instantiate(EquipedSpell, transform);
            EquipedSpell.OnEquip();
        }

        // --------------------------------------------------------------

        void Update()
        {

            //if (m_FireAction.GetStateDown(m_Pose.inputSource))
            //{
            //    EquipedSpell.OnTriggerDown();
            //    Debug.Log("Fire");
            //    CastingSpell = true; 
            //}

            //if (CastingSpell)
            //{
            //    //EquipedSpell.OnTriggerHeld();
            //}

            //if (m_FireAction.GetStateUp(m_Pose.inputSource))
            //{
            //    //EquipedSpell.OnTriggerUp();
            //    CastingSpell = false; 
            //}

        }

        // ------------------------------------------------------------------

    }


}




