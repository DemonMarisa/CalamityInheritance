using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer.Dash
{
    public struct CIDashHitContext
    {
        public int BaseDamage;

        public float BaseKnockback;

        public DamageClass damageClass;

        public int PlayerImmunityFrames;

        public int HitDirection;
    }
}
