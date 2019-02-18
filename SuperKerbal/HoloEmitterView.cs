using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.IO;
using Experience;
using KIS;

/*
Source code copyrighgt 2016, by Martystu Kerman
License: GNU General Public License Version 3
License URL: http://www.gnu.org/licenses/
*/
namespace SuperKerbal
{
    public delegate void SetTraitDelegate(string traitName);
    public delegate void ToggleHighlightDelegate(bool isHighlighted);
    public delegate void SetHighlightColorDelegate(Color color);

    public class HoloEmitterView : Window<HoloEmitterView>
    {
        public SetTraitDelegate setTrait;
        public ToggleHighlightDelegate toggleHighlight;
        public SetHighlightColorDelegate setHighlightColor;

        public string currentTraitName = string.Empty;
        public bool isHighlighted = false;
        public Color highlightColor;
        public ExperienceSystemConfig expSysConfig;
        public ProtoCrewMember crewMember;
        public KerbalEVA kerbalEVA;
        public Part part;

        private Vector2 scrollPos = new Vector2();
        private string[] traitNames;
        float tempRedColor;
        float tempGreenColor;
        float tempBlueColor;
        bool helmetVisible;

        public HoloEmitterView() :
        base("Mobile Emitter", 300, 400)
        {
            Resizable = false;
        }

        public override void SetVisible(bool newValue)
        {
            base.SetVisible(newValue);

            //Get trait names
            expSysConfig = new ExperienceSystemConfig();
            expSysConfig.LoadTraitConfigs();
            traitNames = expSysConfig.TraitNames.ToArray();

            //Reset the helmet
            if (!newValue)
            {

            }
        }

        protected override void DrawWindowContents(int windowId)
        {
            GUILayout.BeginVertical();

            GUILayout.Label("<color=white>Please state the nature of your in-flight emergency:</color>");

            //Toggle highlight
            if (toggleHighlight != null)
            {
                if (GUILayout.Button("Hologram highlighting on/off"))
                {
                    isHighlighted = !isHighlighted;
                    toggleHighlight(isHighlighted);
                }
            }

            //Set highlight color
            GUILayout.BeginHorizontal();
            GUILayout.Label("<color=white>R</color>");
            tempRedColor = highlightColor.r;
            tempRedColor = GUILayout.HorizontalSlider(tempRedColor, 0f, 1f);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("<color=white>G</color>");
            tempGreenColor = highlightColor.g;
            tempGreenColor = GUILayout.HorizontalSlider(tempGreenColor, 0f, 1f);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("<color=white>B</color>");
            tempBlueColor = highlightColor.b;
            tempBlueColor = GUILayout.HorizontalSlider(tempBlueColor, 0f, 1f);
            GUILayout.EndHorizontal();

            if (highlightColor.r != tempRedColor || highlightColor.g != tempGreenColor || highlightColor.b != tempBlueColor)
            {
                highlightColor = new Color(tempRedColor, tempGreenColor, tempBlueColor, 1.0f);
                setHighlightColor(highlightColor);
            }
            
            //Toggle badS
            crewMember.isBadass = GUILayout.Toggle(crewMember.isBadass, "Activate Fearless subroutine");

            //Toggle helmet
            helmetVisible = kerbalEVA.helmetTransform.gameObject.activeSelf;
            if (helmetVisible)
            {
                if (GUILayout.Button("Remove EVA Gear"))
                {
                    setupSuitMeshes(false);
                }
            }
            else
            {
                if (GUILayout.Button("Wear EVA Gear"))
                {
                    setupSuitMeshes(true);
                }
            }

            //Set trait
            GUILayout.Label("<color=white>Current Program: " + currentTraitName + "</color>");
            GUILayout.Label("<color=white>Activate emergency program:</color>");
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            for (int index = 0; index < traitNames.Length; index++)
            {
                if (GUILayout.Button(traitNames[index]) && setTrait != null)
                {
                    currentTraitName = traitNames[index];
                    setTrait(traitNames[index]);
                }
            }
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        void setupSuitMeshes(bool isVisible)
        {
            Collider collider;

            //Toggle helmet
            kerbalEVA.helmetTransform.gameObject.SetActive(isVisible);
            collider = kerbalEVA.helmetTransform.gameObject.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = isVisible;

            //Toggle neck ring
            kerbalEVA.neckRingTransform.gameObject.SetActive(isVisible);
            collider = kerbalEVA.neckRingTransform.gameObject.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = isVisible;

            //Fire event
            GameEvents.OnHelmetChanged.Fire(kerbalEVA, isVisible, isVisible);
        }
    }
}
