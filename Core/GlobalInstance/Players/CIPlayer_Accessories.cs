using CalamityInheritance.Content.Items.Accessories.Combat;
using CalamityInheritance.Content.Items.Accessories.Defense;
using CalamityInheritance.Content.Items.Accessories.Misc;
using CalamityInheritance.Content.Items.Accessories.Restorative;
using CalamityInheritance.Content.Projectiles.Typeless.General;
using CalamityInheritance.Core.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public bool AeroStonePower;
        public bool BeeFriendly;
        public bool Abaddon;
        public bool FungalCarapaceHurt;
        public bool AmidiasSpark;
        public bool FungalSymbiote;
        public bool UnstableGraniteCore;
        public bool PlagueHiveBee;
        public bool ToxicHeart;
        public bool BloodPack;
        public bool DeificAmuletFallenStar;
        public bool RampartOfDeitiesStar;
        public bool CIsponge;
        public bool CIspongeVanity;
        public bool FleshTotem;
        // 翅膀
        public bool AncientAeroWings;
        public void ResetAccessories()
        {
            AeroStonePower = false;
            BeeFriendly = false;
            Abaddon = false;
            FungalCarapaceHurt = false;
            AmidiasSpark = false;
            FungalSymbiote = false;
            UnstableGraniteCore = false;
            PlagueHiveBee = false;
            ToxicHeart = false;
            BloodPack = false;
            DeificAmuletFallenStar = false;
            RampartOfDeitiesStar = false;
            CIsponge = false;
            FleshTotem = false;
            CIspongeVanity = false;
            // 翅膀
            AncientAeroWings = false;
        }
        public void PostUpdateMiscEffect_Accessories()
        {
            if (ToxicHeart)
                ToxicHeartLegacy.UpdateMiscEffect_ToxicHeart(this);
        }
        public void ModifyHurt_Accessories(ref Player.HurtModifiers modifiers, ref float damageMult)
        {
            if (BloodPack)
                BloodPactLegacy.ModfiyHurt(Player, ref modifiers, ref damageMult);
            if (FleshTotem)
                CoreOfTheBloodGod.FleshTotem_PreHurt(Player);
        }
        public void OnHurt_Accessories(Player.HurtInfo info)
        {
            if (FungalCarapaceHurt)
                FungalCarapace.Hurt(info, Player);
            if (AmidiasSpark)
                AmidiasSpark_Hurt(info);
            if (DeificAmuletFallenStar)
                DeificAmuletLegacy.DeificAmuletLegacy_OnHurt(this);
            if (RampartOfDeitiesStar)
                RampartofDeities.RampartOfDeitiesStar_OnHurt(this);
        }
        public void OnHitNPCWithProj_Accessories(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Abaddon)
                AbaddonLegacy.OnHitNPCWithProj_Abaddon(this, proj, target, hit);
        }
        public void OnHitNPC_Accessories(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (UnstableGraniteCore)
                UnstableGraniteCoreLegacy.OnHitNPC_UGC(this, target, hit, damageDone);
            if (PlagueHiveBee)
                PlagueHive.OnHitNPC_PlagueHive(Player, target, hit);
        
        }
        #region 其它饰品
        public void AmidiasSpark_Hurt(Player.HurtInfo info)
        {
            if (info.Damage > 0)
            {
                SoundEngine.PlaySound(SoundID.Item93, Player.Center);
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;

                // Start with base damage, then apply the best damage class you can
                int sDamage = 6;
                if (Main.hardMode)
                    sDamage += 42;
                sDamage = (int)Player.CalcIntDamage<GenericDamageClass>(sDamage);

                if (Player.whoAmI == Main.myPlayer)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                        int spark1 = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<ElectricSpark>(), sDamage, 1.25f, Player.whoAmI, 0f, 1);
                        int spark2 = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<ElectricSpark>(), sDamage, 1.25f, Player.whoAmI, 0f, 1);
                        Main.projectile[spark1].timeLeft = 120;
                        Main.projectile[spark2].timeLeft = 120;
                    }
                }
            }
        }
        #endregion
    }
}
