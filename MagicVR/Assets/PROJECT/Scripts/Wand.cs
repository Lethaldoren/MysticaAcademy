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
        public Vector3 velocity;
        public Vector3 angularVelocity;

        // private variables
        private SteamVR_Behaviour_Pose m_Pose = null;
        public bool castingSpell;

        [SerializeField]
        private GameObject equipedSpellObject;
        // the currently equiped spell as Spell
        [SerializeField]
        private Spell equipedSpell;
        [SerializeField]
        private string equipedSpellName;
        // the list of all spells words
        private string[] spellWords;

        // this is what reconizes voice commands and activates the functions
        private KeywordRecognizer keywordRecognizer;

        // -------------------------------------------------------

        void Start()
        {
            //get all spell words from spell manager
            // try
            // {
            //     spellWords = SpellManager.Instance.GetSpellWords();
            // }
            // catch (System.Exception)
            // {
            //     Debug.LogError("Cannot load spells, check SpellManager for any empty entries");
            //     throw;
            // }

            keywordRecognizer = new KeywordRecognizer(SpellManager.Instance.spellDict.Keys.ToArray()); // this adds all actions in the dictionary to the voice commands the keywordRecognizer will listen for
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
                Destroy(equipedSpellObject);

                // try fetching the spell
                equipedSpellObject = SpellManager.Instance.GetSpell(spellName);
                equipedSpell = equipedSpellObject.GetComponent<Spell>();
                if (equipedSpell)
                {
                    Instantiate(equipedSpellObject, transform);
                    equipedSpell.OnEquip.Invoke();
                    equipedSpell.origin = m_WandTip;
                    equipedSpell.wand = this;
                    Debug.Log(equipedSpell.magicWords + " equipped!");
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
            // [DEBUG] Equipping spells
            if (Input.GetKeyDown(KeyCode.F))
            {
                EquipSpell("Fire Ball");
            }
            // if (Input.GetKeyDown(KeyCode.G))
            // {
            //     EquipSpell("Wind Slash");
            // }
#endif

            // Casting inputs
            // Down
#if DEBUG_INPUT
            if (Input.GetKeyDown(KeyCode.Space)) // DEBUG INPUT
#else
            if (m_FireAction.GetStateDown(m_Pose.inputSource))
#endif
            {
                equipedSpell.OnTriggerDown.Invoke();
                castingSpell = true;
            }

            // Held
#if DEBUG_INPUT
            if (castingSpell && Input.GetKey(KeyCode.Space)) // DEBUG INPUT
#else
            if (castingSpell && m_FireAction.GetState(m_Pose.inputSource))
#endif
            {
                equipedSpell.OnTriggerHeld.Invoke();
            }

            // Up
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

        void FixedUpdate()
        {
#if !DEBUG_INPUT
            m_Pose.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
#endif
        }

        // ------------------------------------------------------------------

        // Component CopyComponent(Component original, GameObject destination)
        // {
        //     System.Type type = original.GetType();
        //     Component copy = destination.AddComponent(type);
        //     // Copied fields can be restricted with BindingFlags
        //     System.Reflection.FieldInfo[] fields = type.GetFields();
        //     foreach (System.Reflection.FieldInfo field in fields)
        //     {
        //         field.SetValue(copy, field.GetValue(original));
        //     }
        //     return copy;
        // }
    }


}




