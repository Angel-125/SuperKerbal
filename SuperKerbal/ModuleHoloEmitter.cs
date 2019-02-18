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
    /*
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class SuperKerbalAddon : MonoBehaviour
    {
        public void Awake()
        {
            Part evaPrefab = PartLoader.getPartInfoByName("kerbalEVA").partPrefab;
            try { evaPrefab.AddModule("ModuleHoloEmitter"); }
            catch { }

            evaPrefab = PartLoader.getPartInfoByName("kerbalEVAfemale").partPrefab;
            try { evaPrefab.AddModule("ModuleHoloEmitter"); }
            catch { }
        }
    }
     */

    public class ModuleHoloEmitter : ModuleKISItem
    {
        [KSPField(isPersistant = true)]
        public string currentTraitName = string.Empty;

        [KSPField(isPersistant = true)]
        public bool isHighlighted = false;

        [KSPField(isPersistant = true)]
        public Color highlightColor = new Color(0, 191, 243);

        protected ExperienceSystemConfig expSysConfig;
        protected HoloEmitterView holoEmitterView;
        protected KerbalEVA kerbalEVA;

        [KSPEvent(guiName = "Activate Emitter", guiActive = true)]
        public void ActivateEmitter()
        {
            Vessel vessel = FlightGlobals.ActiveVessel;
            ProtoCrewMember crewMember = vessel.GetVesselCrew()[0];

            //Show the holo emitter screen.
            holoEmitterView.crewMember = crewMember;
            holoEmitterView.currentTraitName = currentTraitName;
            holoEmitterView.isHighlighted = isHighlighted;
            holoEmitterView.highlightColor = this.highlightColor;
            holoEmitterView.kerbalEVA = this.kerbalEVA;
            holoEmitterView.part = this.part;
            if (holoEmitterView.IsVisible())
                holoEmitterView.SetVisible(false);
            else
                holoEmitterView.SetVisible(true);
        }

        public void OnGUI()
        {
            if (!HighLogic.LoadedSceneIsFlight)
                return;
            if (!holoEmitterView.IsVisible())
                return;

            holoEmitterView.DrawWindow();
        }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            if (HighLogic.LoadedSceneIsFlight == false)
                return;

            //Get kerbalEVA
            this.kerbalEVA = this.part.vessel.FindPartModuleImplementing<KerbalEVA>();

            //Set up the system config
            expSysConfig = new ExperienceSystemConfig();
            expSysConfig.LoadTraitConfigs();

            //Setup view
            holoEmitterView = new HoloEmitterView();
            holoEmitterView.toggleHighlight = ToggleHighlight;
            holoEmitterView.setTrait = SetTrait;
            holoEmitterView.setHighlightColor = SetHighlightColor;

            //Set current trait if needed
            if (string.IsNullOrEmpty(currentTraitName))
            {
                Vessel vessel = FlightGlobals.ActiveVessel;
                ProtoCrewMember crewMember = vessel.GetVesselCrew()[0];

                currentTraitName = crewMember.trait;
            }

            //Set highlighting and such
            ToggleHighlight(isHighlighted);
            SetTrait(currentTraitName);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (HighLogic.LoadedSceneIsFlight == false)
                return;

            if (isHighlighted)
                this.part.vessel.rootPart.Highlight(highlightColor);
        }

        public void SetHighlightColor(Color color)
        {
            this.highlightColor = color;

            if (isHighlighted)
                this.part.vessel.rootPart.Highlight(highlightColor);
        }

        public void ToggleHighlight(bool isHighlighted)
        {
            this.isHighlighted = isHighlighted;

            //Turn off the highlighting if it's been toggled off.
            if (isHighlighted == false)
            {
                this.part.Highlight(false);
                return;
            }

            //Highlight the part.
            this.part.vessel.rootPart.Highlight(highlightColor);
        }

        public void SetTrait(string traitName)
        {
            Vessel vessel = FlightGlobals.ActiveVessel;
            ProtoCrewMember crewMember = vessel.GetVesselCrew()[0];

            if (expSysConfig.TraitNames.Contains(traitName) == false)
                return;

            //Set the kerbal experience trait
            currentTraitName = traitName;
        }
    }
}
