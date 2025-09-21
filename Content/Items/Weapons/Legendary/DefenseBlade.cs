using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Core;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class DefenseBlade: CIMelee, ILocalizedModType
    {
        
        public static string TextRoute => $"{Generic.WeaponTextPath}.Melee.DefenseBlade";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public int baseDamage = 75;
        public override void SetDefaults()
        {
            Item.height = Item.width = 72;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.damage = baseDamage;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = Item.useTime = 10;
            Item.scale *= 1.25f;
            Item.useTurn = true;
            Item.knockBack = 4.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 14f;
            Item.rare = CIConfig.Instance.LegendaryRarity ? ModContent.RarityType<GolemPurple>() : ModContent.RarityType<MaliceChallengeDrop>();
            Item.value = CIShopValue.RarityMaliceDrop;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.DamageType = DamageClass.Melee;
                //谁家好人射弹剑刀刃不造成伤害啊?
                Item.noMelee = false;
                Item.useTurn = false;
                Item.UseSound = SoundID.Item73;
                Item.shoot = ModContent.ProjectileType<DefenseBeam>();
            }
            else
            {
                Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
                Item.noMelee = false;
                Item.useTurn = true;
                Item.UseSound = SoundID.Item1;
                Item.shoot = ProjectileID.None;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
                Projectile.NewProjectile(source, position, velocity, type, damage / 2, knockback, player.whoAmI);
            else
                return false;

            return false; // Return false because we've manually created the projectiles.
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
             if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GoldCoin, 0f, 0f, 0, new Color(255, Main.DiscoG, 53));
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            string t1 = mp.DefendTier1? Language.GetTextValue($"{TextRoute}.TierOne") : Language.GetTextValue($"{TextRoute}.TierOneTint");
            tooltips.FindAndReplace("[TIERONE]", t1);
            string t2 = mp.DefendTier2? Language.GetTextValue($"{TextRoute}.TierTwo") : Language.GetTextValue($"{TextRoute}.TierTwoTint");
            tooltips.FindAndReplace("[TIERTWO]", t2);
            string t3 = mp.DefendTier3? Language.GetTextValue($"{TextRoute}.TierThree") : Language.GetTextValue($"{TextRoute}.TierThreeTint");
            tooltips.FindAndReplace("[TIERTHREE]", t3);
            //用于发送传奇武器在至尊灾厄眼在场时得到数值增强的信息
            string t4 = null;
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                t4 = Language.GetTextValue($"{Generic.WeaponTextPath}.EmpoweredTooltip.Generic");
            //以下，用于比较复杂的计算
            float getDmg = LegendaryDamage() + Generic.GenericLegendBuffInt();
            int boostPercent = (int)(getDmg);
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
            if (t4 != null)
            tooltips.Add(new TooltipLine(Mod, "Buff", t4));
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(baseDamage + LegendaryDamage() + Generic.GenericLegendBuffInt(1000)) / (float)baseDamage);
            damage *= Buff;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) => Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<DefenseBlast>(), Item.damage, Item.knockBack, Main.myPlayer);
        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) => Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<DefenseBlast>(), Item.damage, Item.knockBack, Main.myPlayer);
        public static float LegendaryDamage()
        {
            int dmgBuff = 0;
            dmgBuff += Condition.DownedEmpressOfLight.IsMet() ? 5 : 0; // 80
            dmgBuff += Condition.DownedDukeFishron.IsMet() ? 5 : 0;    // 85
            dmgBuff += DownedBossSystem.downedRavager ? 5 : 0;         // 90
            dmgBuff += DownedBossSystem.downedPlaguebringer ? 5 : 0;   // 95
            dmgBuff += Condition.DownedCultist.IsMet() ? 40 : 0;       // 135
            dmgBuff += DownedBossSystem.downedAstrumDeus ? 20 : 0;     // 135
            dmgBuff += Condition.DownedMoonLord.IsMet() ? 185 : 0;     // 320
            dmgBuff += DownedBossSystem.downedGuardians ? 20 : 0;      // 340
            dmgBuff += DownedBossSystem.downedProvidence ? 160 : 0;    // 500
            dmgBuff += DownedBossSystem.downedSignus ? 30 : 0;         // 530
            dmgBuff += DownedBossSystem.downedCeaselessVoid ? 30 : 0;  // 560
            dmgBuff += DownedBossSystem.downedStormWeaver ? 30 : 0;    // 590
            dmgBuff += DownedBossSystem.downedPolterghast ? 210 : 0;   // 800
            dmgBuff += DownedBossSystem.downedBoomerDuke ? 40 : 0;     // 840
            dmgBuff += DownedBossSystem.downedDragonfolly ? 20 : 0;    // 860
            dmgBuff += DownedBossSystem.downedDoG ? 1600 : 0;          // 2460
            dmgBuff += DownedBossSystem.downedYharon ? 2540 : 0;       // 5000
            dmgBuff += DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas && DownedBossSystem.downedPrimordialWyrm && CIDownedBossSystem.DownedLegacyScal ? 5000 : 0; //9200
            return dmgBuff;
        }

        
    }
}