using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamityInheritance.Utilities;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class SoulSmallPlaceholder: ShizukuBaseGhost, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override int ProjHeight => 30;
        public override int ProjWidth => 30;
        public override float TimeToHoming => 15f;
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return new(68, 216, 229, 255);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 60);
        }
    }
}
