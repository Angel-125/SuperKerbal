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
    public delegate void ToggleHelmetDelegate(bool showHelmet);

    public class HoloEmitterView : Window<HoloEmitterView>
    {
        public SetTraitDelegate setTrait;
        public ToggleHighlightDelegate toggleHighlight;
        public ToggleHelmetDelegate setHelmetVisible;

        public string currentTraitName = string.Empty;
        public bool isHighlighted = false;
        public bool showHelmet = true;
        public ExperienceSystemConfig expSysConfig;
        public ProtoCrewMember crewMember;

        private Vector2 scrollPos = new Vector2();
        private string[] traitNames;

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

            //Toggle badS
            crewMember.isBadass = GUILayout.Toggle(crewMember.isBadass, "Activate Fearless subroutine");

            //Toggle helmet
            if (setHelmetVisible != null)
            {
                showHelmet = GUILayout.Toggle(showHelmet, "Render helmet");
                setHelmetVisible(showHelmet);
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
    }
}
