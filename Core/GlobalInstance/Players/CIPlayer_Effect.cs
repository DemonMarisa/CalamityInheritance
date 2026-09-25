using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public Dictionary<string, Action> CIEffects = new Dictionary<string, Action>();
        public void UpdateEffect_PostUpdateMisc()
        {
            foreach (var key in CIEffects.Values)
                key.Invoke();
            CIEffects.Clear();
        }
        public void AddEffect(string key, Action action)
        {
            if (!HasEffect(key))
                CIEffects.TryAdd(key, action);
        }
        public bool HasEffect(string key)
        {
            return CICD.ContainsKey(key);
        }
    }
}
