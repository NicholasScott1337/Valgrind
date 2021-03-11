using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Shared
{
    [HarmonyPatch(typeof(Pickable), "SetPicked", null)]
    class Patch_Pickable_SetPicked : SmartBepInMods.Tools.Patching.Constants.SHARED
	{

		// Token: 0x06000003 RID: 3 RVA: 0x00002078 File Offset: 0x00000278
		[HarmonyPrefix]
		public static void Prefix(ZNetView ___m_nview, bool ___m_picked, ref Patch_Pickable_SetPicked.PickState __state)
		{
			__state = new Patch_Pickable_SetPicked.PickState();
			__state.picked_time = ___m_nview.GetZDO().GetLong("picked_time", 0L);
			__state.picked = ___m_picked;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020A4 File Offset: 0x000002A4
		[HarmonyPostfix]
		public static void Postfix(ZNetView ___m_nview, bool ___m_picked, ref Patch_Pickable_SetPicked.PickState __state)
		{
			bool flag = __state != null && __state.picked == ___m_picked && ___m_nview.IsOwner();
			if (flag)
			{
				___m_nview.GetZDO().Set("picked_time", __state.picked_time);
			}
		}

		// Token: 0x02000004 RID: 4
		public class PickState
		{
			// Token: 0x04000002 RID: 2
			public long picked_time;

			// Token: 0x04000003 RID: 3
			public bool picked;
		}
	}
}
