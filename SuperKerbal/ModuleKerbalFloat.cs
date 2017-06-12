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
    public class ModuleKerbalFloat : ModuleKISItem
    {
        [KSPField]
        public float buoyancy = 1.2f;

        protected float originalBuoyancy;

        public override void OnEquip(KIS_Item item)
        {
            base.OnEquip(item);
            try
            {
                originalBuoyancy = item.inventory.part.buoyancy;
                item.inventory.part.buoyancy = buoyancy;
            }
            catch (Exception ex)
            {
                Debug.Log("[ModuleKerbalFloat] - Oops: " + ex);
            }
        }

        public override void OnUnEquip(KIS_Item item)
        {
            base.OnUnEquip(item);

            try
            {
                item.inventory.part.buoyancy = originalBuoyancy;
            }
            catch (Exception ex)
            {
                Debug.Log("[ModuleKerbalFloat] - Oops: " + ex);
            }
        }
    }
}
