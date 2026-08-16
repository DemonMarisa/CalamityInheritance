using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.Buff.Armor.Misc;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Misc;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public bool GodSlayerReborn = false;
        public bool SilvaReborn = false;
        public int SilvaTimer = 0;
        public void PreKillReset()
        {
            GodSlayerReborn = false;
            SilvaReborn = false;
            if (SilvaTimer > 0)
                SilvaTimer--;
        }
        public void PostUpdate_PreKill()
        {
            if (SilvaTimer > 0)
            {
                Player.SetImmuneTimeForAllTypes(2);
                Player.AddBuff(BuffType<SilvaImmunity>(), 2);
                InvincibleTimer = 2;
                for (int j = 0; j < 2; j++)
                {
                    Dust green = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.ChlorophyteWeapon, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 2f);
                    green.position.X += (float)Main.rand.Next(-20, 21);
                    green.position.Y += (float)Main.rand.Next(-20, 21);
                    green.velocity *= 0.9f;
                    green.noGravity = true;
                    green.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                    green.shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                    if (Main.rand.NextBool())
                        green.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                }
                if (SilvaTimer == 1)
                    Player.AddCD(LAPContent.CDType<SilvaReviveCD>(), 60 * 180);
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (GodSlayerReborn && !Player.HasCD<GodSlayerCooldown>())
            {
                GSReborn();
                return false;
            }
            if (SilvaReborn && !Player.HasCD<SilvaReviveCD>())
            {
                SReborn();
                return false;
            }
            return true;
        }
        public void GSReborn()
        {
            SoundEngine.PlaySound(CISoundID.SoundRainbowGun, Player.Center);
            for (int j = 0; j < 50; j++)
            {
                int nebulousReviveDust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
                Dust dust = Main.dust[nebulousReviveDust];
                dust.position.X += Main.rand.Next(-20, 21);
                dust.position.Y += Main.rand.Next(-20, 21);
                dust.velocity *= 0.9f;
                dust.scale *= 1f + Main.rand.Next(40) * 0.01f;
                // Change this accordingly if we have a proper equipped sprite.
                dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cBody, Player);
                if (Main.rand.NextBool())
                    dust.scale *= 1f + Main.rand.Next(40) * 0.01f;
            }
            Player.statLife = +100;
            if (DraconicSurge)
                Player.NCHeal(Player.statLifeMax2);
            Player.AddCD(LAPContent.CDType<GodSlayerCooldown>(), 30 * 60);
        }
        public void SReborn()
        {
            SoundEngine.PlaySound(CISounds.SilvaActivation, Player.Center);
            SilvaTimer = 900;
        }
    }
}
