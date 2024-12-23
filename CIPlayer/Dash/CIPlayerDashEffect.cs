using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;

namespace CalamityInheritance.CIPlayer.Dash
{
    public abstract class CIPlayerDashEffect
    {
        // For the sake of easy access without the need for stored instances, this property is defined as static instead of abstract.
        // Derived classes should use the new keyword on this member to define their own version of it.
        public static string ID { get; }

        public abstract DashCollisionType CollisionType { get; }

        public abstract bool IsOmnidirectional { get; }

        public abstract float CalculateDashSpeed(Player player);

        public virtual void OnDashEffects(Player player) { }

        public virtual void MidDashEffects(Player player, ref float dashSpeed, ref float dashSpeedDecelerationFactor, ref float runSpeedDecelerationFactor) { }

        public virtual void OnHitEffects(Player player, NPC npc, IEntitySource source, ref CIDashHitContext hitContext) { }
    }
}
