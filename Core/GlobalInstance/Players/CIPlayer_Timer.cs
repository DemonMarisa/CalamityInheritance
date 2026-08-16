using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public int soundDelay = 0;
        public int GlobalHealProjCD;
        public Dictionary<string, int> CICD = new Dictionary<string, int>();
        public void UpdateTimer()
        {
            if (soundDelay > 0)
                soundDelay--;
            if (GlobalHealProjCD > 0)
                GlobalHealProjCD--;
            foreach (var key in CICD.Keys.ToList())
            {
                if (CICD[key] > 0)
                    CICD[key]--;

                if (CICD[key] <= 0)
                    CICD.Remove(key);
            }
        }
        public void AddCount(string key, int count)
        {
            CICD.TryAdd(key, count);
        }
        public bool HasCount(string key)
        {
            return CICD.ContainsKey(key);
        }
        public void ResetTimerDeath()
        {
            soundDelay = 0;
            GlobalHealProjCD = 0;
        }
    }
}
