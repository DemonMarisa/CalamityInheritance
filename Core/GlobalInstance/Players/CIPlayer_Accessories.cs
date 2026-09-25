using CalamityInheritance.Content.Buff.Buffs;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Items.Accessories.Combat;
using CalamityInheritance.Content.Items.Accessories.Defense;
using CalamityInheritance.Content.Items.Accessories.Misc;
using CalamityInheritance.Content.Items.Accessories.Movement;
using CalamityInheritance.Content.Items.Accessories.Professional;
using CalamityInheritance.Content.Items.Accessories.Restorative;
using CalamityInheritance.Content.Projectiles.Typeless.General;
using CalamityInheritance.Core.Utils;
using LAP.Core.IDSets;
using LAP.Core.NetCode.NetUtilities;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public bool DealHolyFire;
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
        public bool AstralArcanumRegen;
        public bool projRef;
        public bool AbyssalAmuletLegacy;
        public bool LuxorsGiftLegacyShoot;
        public bool ElysianAegis;
        public bool ElysianGuard;
        public bool RegenatorLegacy;
        public bool ManaOverloaderHeal;
        public bool ElemGauntlet;
        public bool ElemQuiver;
        public int ElemQuiverSpiltStyle;
        public bool NanoTech;
        public bool EclispeMirror;
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
            AstralArcanumRegen = false;
            projRef = false;
            AbyssalAmuletLegacy = false;
            LuxorsGiftLegacyShoot = false;
            ElysianAegis = false;
            RegenatorLegacy = false;
            ManaOverloaderHeal = false;
            ElemQuiver = false;
            NanoTech = false;
            EclispeMirror = false;
            // 翅膀
            AncientAeroWings = false;
        }
        public void PostUpdateMiscEffect_Accessories()
        {
            if (ToxicHeart)
                ToxicHeartLegacy.UpdateMiscEffect_ToxicHeart(this);
            if (LuxorsGiftLegacyShoot)
                LuxorsGiftLegacy.PostUpdate(Player);
            if (ElysianAegis)
                ElysianAegisold.ElysianAegis_PostUpdate(this);
            if (RegenatorLegacy)
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.5f);
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
        public void ModifyHitNPC_Accessories(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (modifiers.DamageType.CountsAsClass<ThrowingDamageClass>())
            {
                if (NanoTech)
                    modifiers.SetCrit();
            }
            if (CritDamageAdd != 0)
                modifiers.CritDamage += CritDamageAdd;
        }
        public void OnHitNPCWithProj_Accessories(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Abaddon)
                AbaddonLegacy.OnHitNPCWithProj_Abaddon(this, proj, target, hit);
            if (ManaOverloaderHeal)
                ManaOverloader.HitNPC(target, proj, damageDone, Player);
        }
        public void OnHitNPC_Accessories(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (UnstableGraniteCore)
                UnstableGraniteCoreLegacy.OnHitNPC_UGC(this, target, hit, damageDone);
            if (PlagueHiveBee)
                PlagueHive.OnHitNPC_PlagueHive(Player, target, hit);
            if (AbyssalAmuletLegacy)
                target.AddBuff(BuffType<CICrushDepth>(), 180, false);
            if (DealHolyFire)
                target.AddBuff(BuffType<CIHolyFlames>(), 180, false);
            if (ElemGauntlet)
            {
                target.AddBuff(BuffType<CIElementalMix>(), 300, false);
                target.AddBuff(BuffType<CIBrimstoneFlames>(), 300, false);
                target.AddBuff(BuffType<CIGodSlayerInferno>(), 300, false);
                target.AddBuff(BuffID.Frostburn2, 300);
                target.AddBuff(BuffID.CursedInferno, 300);
                target.AddBuff(BuffID.Inferno, 300);
                target.AddBuff(BuffID.Venom, 300);
            }

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
        public void ProjRef(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (!proj.hostile || !proj.active || Player.HasCD<ProjRefCD>())
                return;
            FreeDodgeThisDamage = true;
            Player.AddCD(LAPContent.CDType<ProjRefCD>(), SecondsToFrames(30));
            Player.SetImmuneTimeForAllTypes(60);
            if (proj.velocity != Vector2.Zero && !LAPIDSet.ProtectedProj.Contains(proj.type) && !LAPIDSet.CantReflectProj.Contains(proj.type) && ProjectileID.Sets.DrawScreenCheckFluff[proj.type] < 500)
            {
                Vector2 shieldNormal = LAPUtilities.GetVector2(proj.Center, proj.Center);
                proj.velocity = proj.velocity - 2f * Vector2.Dot(proj.velocity, shieldNormal) * shieldNormal;
            }
            proj.LAP().BeParry = true;
            proj.damage = 0;
            proj.netSpam = 0;
            proj.netUpdate = true;
            proj.SyncedParryProj();
        }
        #endregion
    }
}
