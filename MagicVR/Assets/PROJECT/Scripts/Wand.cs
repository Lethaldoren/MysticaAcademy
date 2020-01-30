using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;


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

        // the currently equiped spell
        [SerializeField]
        private EquipableSpell EquipedSpell;
        [SerializeField]
        private string EquipedSpellName;

        // the list of all spells
        private EquipableSpell[] allSpells;
        private List<string> spellWords = new List<string>();

        // this is what reconizes voice commands and activates the functions
        private KeywordRecognizer keywordRecognizer;

        // -------------------------------------------------------

        void Start()
        {
            //get all spell words from spell manager
            allSpells = SpellManager.Instance.spellList;
            spellWords = allSpells.Select(s => s.magicWords).ToList();

            keywordRecognizer = new KeywordRecognizer(spellWords.ToArray()); // this adds all actions in the dictionary to the voice commands the keywordRecognizer will listen for
            keywordRecognizer.OnPhraseRecognized += PhraseRecognized;
            keywordRecognizer.Start(); // you can turn the keyword recongnizer on or off

            m_Pose = GetComponentInParent<SteamVR_Behaviour_Pose>();
        }

        // ------------------------------------------------------------

        private void PhraseRecognized(PhraseRecognizedEventArgs speech)
        {
            EquipSpell(speech.text);
        }

        private void EquipSpell(string spellName)
        {
            if (!CastingSpell)
            {
                if (EquipedSpell) EquipedSpell.OnUnequip();

                EquipedSpell = allSpells.First(s => s.magicWords == spellName);
                Debug.Log("Spell Equiped! : " + spellName);

                EquipedSpell = Instantiate(EquipedSpell, transform).GetComponent<EquipableSpell>();
                print(EquipedSpell.GetType() + " | " + EquipedSpell.name);
                EquipedSpell.OnEquip();
            }
        }

        // -------------------------------------------------------------

        void Update()
        {
            // [DEBUG] Equip fireball
            // if (Input.GetKeyDown(KeyCode.F))
            // {
            //     EquipSpell("Fire Ball");
            // }

            if (m_FireAction.GetStateDown(m_Pose.inputSource))
            // if (Input.GetKeyDown(KeyCode.Space)) // DEBUG INPUT
            {
                EquipedSpell.OnTriggerDown();
                Debug.Log("Fire");
                CastingSpell = true;
            }

            if (CastingSpell && m_FireAction.GetState(m_Pose.inputSource))
            // if (Input.GetKey(KeyCode.Space)) // DEBUG INPUT
            {
                EquipedSpell.OnTriggerHeld();
            }

            if (m_FireAction.GetStateUp(m_Pose.inputSource))
            // if (Input.GetKeyUp(KeyCode.Space)) // DEBUG INPUT
            {
                EquipedSpell.OnTriggerUp();
                CastingSpell = false;
            }

        }

        // ------------------------------------------------------------------

    }


}




