using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.IO;

/*
Source code copyrighgt 2016, by Martystu Kerman
License: CC BY-NC-SA 4.0
License URL: https://creativecommons.org/licenses/by-nc-sa/4.0/
*/
namespace SuperKerbal
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class SuperKerbalAddon : MonoBehaviour
    {
        public void Awake()
        {
            Part evaPrefab = PartLoader.getPartInfoByName("kerbalEVA").partPrefab;
            try { evaPrefab.AddModule("TraitSwitcher"); }
            catch { }

            evaPrefab = PartLoader.getPartInfoByName("kerbalEVAfemale").partPrefab;
            try { evaPrefab.AddModule("TraitSwitcher"); }
            catch { }
        }
    }

    public class TraitSwitcher : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveUnfocused = true, unfocusedRange = 3.0f, guiName = "Toggle Skill")]
        public void ToggleTrait()
        {
            ProtoCrewMember crewMember = this.part.protoModuleCrew.First<ProtoCrewMember>();

            switch (crewMember.trait)
            {
                case "Pilot":
                    KerbalRoster.SetExperienceTrait(crewMember, "Engineer");
                    Events["ToggleTrait"].guiName = "Set to Scientist";
                    break;
                case "Engineer":
                    KerbalRoster.SetExperienceTrait(crewMember, "Scientist");
                    Events["ToggleTrait"].guiName = "Set to Pilot";
                    break;
                case "Scientist":
                    KerbalRoster.SetExperienceTrait(crewMember, "Tourist");
                    Events["ToggleTrait"].guiName = "Set to Pilot";
                    break;
                case "Tourist":
                    KerbalRoster.SetExperienceTrait(crewMember, "Pilot");
                    Events["ToggleTrait"].guiName = "Set to Engineer";
                    break;
                default:
                    break;
            }

            //Update the KIS inventory
            KIS.ModuleKISInventory inventory = this.part.FindModuleImplementing<KIS.ModuleKISInventory>();
            inventory.kerbalTrait = crewMember.trait;
        }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            switch (this.part.protoModuleCrew.First<ProtoCrewMember>().trait)
            {
                case "Pilot":
                    Events["ToggleTrait"].guiName = "Set to Engineer";
                    break;
                case "Engineer":
                    Events["ToggleTrait"].guiName = "Set to Scientist";
                    break;
                case "Scientist":
                    Events["ToggleTrait"].guiName = "Set to Tourist";
                    break;
                case "Tourist":
                    Events["ToggleTrait"].guiName = "Set to Pilot";
                    break;
                default:
                    break;
            }
        }

    }
}
