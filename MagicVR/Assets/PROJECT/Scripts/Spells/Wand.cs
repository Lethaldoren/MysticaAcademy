// Comment and uncomment to toggle debug input
#define DEBUG_INPUT

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
        public bool noVRInput;
        // public variables
        public SteamVR_Action_Boolean m_FireAction = null;
        public Transform m_WandTip = null;
        public Vector3 velocity;
        public Vector3 angularVelocity;

        // private variables
        private SteamVR_Behaviour_Pose m_Pose = null;
        public bool castingSpell;

        [SerializeField]
        private GameObject equipedSpell;

        // this is what reconizes voice commands and activates the functions
        private KeywordRecognizer keywordRecognizer;

        // -------------------------------------------------------

        void Start()
        {
            keywordRecognizer = new KeywordRecognizer(SpellManager.Instance.AllSpellNames()); // this adds all actions in the dictionary to the voice commands the keywordRecognizer will listen for
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
                // unequip spell if there is one
                if (equipedSpell != null)
                {
                    SpellManager.Instance.ReturnSpell(equipedSpell);
                    equipedSpell.GetComponent<Spell>().OnUnequip.Invoke();
                }

                // try fetching the spell
                equipedSpell = SpellManager.Instance.GetSpell(spellName);
                if (equipedSpell)
                {
                    equipedSpell.transform.SetParent(transform, false);
                    // equipedSpell.transform.localPosition = new Vector3(0, 0, .5f);
                    //Debug.Log(spellName + " equipped!");
                    equipedSpell.GetComponent<Spell>().OnEquip.Invoke();
                }
            }
        }

        // -------------------------------------------------------------

        void Update()
        {
            if (equipedSpell != null)
            {
                if (noVRInput)
                {
                    // [DEBUG] Equipping spells
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        EquipSpell("Fire Ball");
                    }
                    if (Input.GetKeyDown(KeyCode.J))
                    {
                        EquipSpell("Jolt");
                    }

                    // Down
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        equipedSpell.GetComponent<Spell>().OnTriggerDown.Invoke();
                        castingSpell = true;
                    }

                    // Held
                    if (castingSpell && Input.GetKey(KeyCode.Space))
                    {
                        equipedSpell.GetComponent<Spell>().OnTriggerHeld.Invoke();
                    }

                    // Up
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        equipedSpell.GetComponent<Spell>().OnTriggerUp.Invoke();
                        castingSpell = false;
                    }
                }
                else
                {
                    // Down
                    if (m_FireAction.GetStateDown(m_Pose.inputSource))
                    {
                        // Debug.Log("down");
                        equipedSpell.GetComponent<Spell>().OnTriggerDown.Invoke();
                        castingSpell = true;
                    }

                    // Held
                    if (castingSpell && m_FireAction.GetState(m_Pose.inputSource))
                    {
                        // Debug.Log("held");
                        equipedSpell.GetComponent<Spell>().OnTriggerHeld.Invoke();
                    }

                    // Up
                    if (m_FireAction.GetStateUp(m_Pose.inputSource))
                    {
                        // Debug.Log("up");
                        equipedSpell.GetComponent<Spell>().OnTriggerUp.Invoke();
                        castingSpell = false;
                    }
                    
                }

            }

        }

        // ------------------------------------------------------------------

        void FixedUpdate()
        {
            // velocity = m_Pose.GetVelocity();
            // angularVelocity = m_Pose.GetAngularVelocity();

            m_Pose.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
            velocity = transform.root.localToWorldMatrix * velocity;
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




