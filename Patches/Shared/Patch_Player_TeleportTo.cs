using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Valgrind.Patches.Shared
{
    [HarmonyPatch(typeof(Player), "TeleportTo", null)]
    class Patch_Player_TeleportTo : SmartBepInMods.Tools.Patching.Constants.SHARED
	{
		public static List<Character> pets = new List<Character>();
		private static void Prefix(Player __instance)
		{
			pets.Clear();
			List<Character> list = new List<Character>();
			Character.GetCharactersInRange(__instance.transform.position, 20f, list);
			int num = 0;
			foreach (Character character in list)
			{
				if (character.IsTamed())
				{
					MonsterAI component = character.GetComponent<MonsterAI>();
					if (component && component.GetFollowTarget() && component.GetFollowTarget().Equals(__instance.gameObject) && ++num <= 4)
					{
						pets.Add(character);
					}
				}
			}
		}
	}
}
