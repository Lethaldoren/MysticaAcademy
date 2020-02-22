// Comment and uncomment to toggle debug input
// #define DEBUG_INPUT

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
        // public variables
        public SteamVR_Action_Boolean m_FireAction = null;
        public Transform m_WandTip = null;

        // private variables
        private SteamVR_Behaviour_Pose m_Pose = null;
        private bool castingSpell;

        // the currently equiped spell
        [SerializeField]
        private Spell equipedSpell;
        [SerializeField]
        private string equipedSpellName;
        // the list of all spells words
        private List<string> spellWords = new List<string>();

        // this is what reconizes voice commands and activates the functions
        private KeywordRecognizer keywordRecognizer;

        // -------------------------------------------------------

        void Start()
        {
            //get all spell words from spell manager
            spellWords = SpellManager.Instance.spellList.Select(s => s.magicWords).ToList();

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
            if (!castingSpell)
            {
                // trigger the unequip event on the existing spell
                equipedSpell?.OnUnequip.Invoke();

                // try fetching the spell
                equipedSpell = SpellManager.Instance.GetSpell(spellName);
                if (equipedSpell)
                {
                    Debug.Log(spellName + " equipped!");

                    // trigger the equip event
                    equipedSpell.OnEquip.Invoke();
                    equipedSpell.origin = m_WandTip;
                }
                else
                {
                    Debug.Log("Couldn't find correct spell.");
                }
            }
        }

        // -------------------------------------------------------------

        void Update()
        {
#if DEBUG_INPUT
            // [DEBUG] Equip fireball
            if (Input.GetKeyDown(KeyCode.F))
            {
                EquipSpell("Fire Ball");
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                EquipSpell("Wind Slash");
            }
#endif

#if DEBUG_INPUT
            if (Input.GetKeyDown(KeyCode.Space)) // DEBUG INPUT
#else
            if (m_FireAction.GetStateDown(m_Pose.inputSource))
#endif
            {
                equipedSpell.OnTriggerDown.Invoke();
                castingSpell = true;
            }

#if DEBUG_INPUT
            if (castingSpell && Input.GetKey(KeyCode.Space)) // DEBUG INPUT
#else
            if (castingSpell && m_FireAction.GetState(m_Pose.inputSource))
#endif
            {
                equipedSpell.OnTriggerHeld.Invoke();
            }

#if DEBUG_INPUT
            if (Input.GetKeyUp(KeyCode.Space)) // DEBUG INPUT
#else
            if (m_FireAction.GetStateUp(m_Pose.inputSource))
#endif
            {
                equipedSpell.OnTriggerUp.Invoke();
                castingSpell = false;
            }

        }

        // ------------------------------------------------------------------

        Component CopyComponent(Component original, GameObject destination)
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy;
        }
    }


}




