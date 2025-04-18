using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.NPCs.Boss.SCAL;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class DukeLegendary: CIMelee, ILocalizedModType
    {
        
        public static string TextRoute => $"{Generic.GetWeaponLocal}.Melee.DukeLegendary";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            ItemID.Sets.BonusAttackSpeedMultiplier[Item.type] = 1.33f;
        }

        public override void SetDefaults()
        {
            Item.width = 100;
            Item.height = 102;
            Item.damage = 75;
            Item.scale *= 1.5f;
            Item.knockBack = 4f;
            Item.useAnimation = Item.useTime = 15;
            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.shootSpeed = 9f;

            Item.shoot = ModContent.ProjectileType<Razorwind>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;

            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = CIConfig.Instance.LegendaryRarity ? ModContent.RarityType<DukeAqua>() : ModContent.RarityType<MaliceChallengeDrop>();
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
            }
            else
            {
                Item.noMelee = false;
            }

            return base.UseItem(player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            string t1 = mp.DukeTier1 ? Language.GetTextValue($"{TextRoute}.TierOne") : Language.GetTextValue($"{TextRoute}.TierOneTint");
            tooltips.FindAndReplace("[TIERONE]", t1);
            string t2 = mp.DukeTier2 ? Language.GetTextValue($"{TextRoute}.TierTwo") : Language.GetTextValue($"{TextRoute}.TierTwoTint");
            tooltips.FindAndReplace("[TIERTWO]", t2);
            string t3 = mp.DukeTier3 ? Language.GetTextValue($"{TextRoute}.TierThree") : Language.GetTextValue($"{TextRoute}.TierThreeTint");
            tooltips.FindAndReplace("[TIERTHREE]", t3);
            //用于发送传奇武器在至尊灾厄眼在场时得到数值增强的信息
            string t4 = null;
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                t4 = Language.GetTextValue($"{Generic.GetWeaponLocal}.EmpoweredTooltip.Generic");
            //以下，用于比较复杂的计算
            float getDmg = LegendaryDamage();
            int boostPercent = (int)(getDmg * 100);
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
            if (t4 != null)
            tooltips.Add(new TooltipLine(Mod, "Buff", t4));
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= LegendaryDamage() + Generic.GenericLegendBuff();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 speed = player.CIMod().DukeTier1 ? velocity * 2f : velocity;
                int realDamage = player.CIMod().DukeTier1? damage : (int)(damage * 0.6);
                Projectile.NewProjectile(source, position, speed, ModContent.ProjectileType<DukeLegendaryRazor>(), realDamage, knockback, player.whoAmI);
                if (player.CIMod().DukeTier2)
                {
                    //遍历最近的NPC
                    NPC target = CIFunction.FindClosestTargetPlayer(player, 1800f, true, true);
                    if (target == null)
                        return false;
                    
                    if (player.CIMod().GlobalFireDelay == 0)
                    {
                        for (int i = 0; i < 4 ; i++)
                        {
                            int j = Main.rand.NextBool() ? -1 : 1;
                            float offset = 800f * j;
                            float posX = player.Center.X + Main.rand.NextFloat(-300f, 300f);
                            //这里得取玩家头顶, 不然敌对单位在玩家底下的时候直接露馅
                            float posY = player.Center.Y - offset;

                            Vector2 pos = new (posX, posY);
                            Vector2 realSpeed = new Vector2(Item.shootSpeed * 3, 0f).RotatedBy(MathHelper.PiOver2 * j);
                            //直接让其从天上降落。
                            Projectile.NewProjectile(source, pos, realSpeed, ModContent.ProjectileType<DukeLegendaryRazorClone>(), damage * 2, knockback, player.whoAmI);
                        }
                        player.CIMod().GlobalFireDelay = 10;
                    }
                }
            }
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                damage = player.CIMod().DukeTier1? damage: (int)(damage * 0.6);
                type = ModContent.ProjectileType<DukeLegendaryRazor>();
            }

            else
                type = ProjectileID.None;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Flare_Blue, 0f, 0f, 100, new Color(53, Main.DiscoG, 255));
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = player.GetSource_ItemUse(Item);
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<DukeLegendaryBubble>(), Item.damage, Item.knockBack, player.whoAmI);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            var source = player.GetSource_ItemUse(Item);
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<DukeLegendaryBubble>(), Item.damage, Item.knockBack, player.whoAmI);
        }

        public override void UseAnimation(Player player)
        {
            Item.noUseGraphic = false;
            Item.UseSound = SoundID.Item1;

            if (player.altFunctionUse == 2)
            {
                Item.noUseGraphic = true;
                Item.UseSound = SoundID.Item84;
            }
        }
        //8个Boss
        public static float LegendaryDamage()
        {
            float damageBuff = 0f;
            //+3个boss
            damageBuff += DownedBossSystem.downedPlaguebringer ? 0.2f : 0f;
            damageBuff += DownedBossSystem.downedRavager ? 0.2f : 0f;
            damageBuff += NPC.downedEmpressOfLight ? 0.2f : 0f;
            damageBuff += NPC.downedAncientCultist ? 0.2f : 0f;
            damageBuff += NPC.downedMoonlord ? 0.2f : 0f;
            damageBuff += DownedBossSystem.downedProvidence ? 0.4f : 0f;
            damageBuff += DownedBossSystem.downedPolterghast ? 0.8f : 0f;
            damageBuff += DownedBossSystem.downedBoomerDuke ? 0.8f : 0f;
            damageBuff += DownedBossSystem.downedDoG ? 1.0f : 0f;
            damageBuff += DownedBossSystem.downedYharon ? 1.2f : 0f;
            damageBuff += DownedBossSystem.downedExoMechs || DownedBossSystem.downedCalamitas? 1f : 0f;
            //恭喜击败至尊灾厄眼，所以。500%?
            damageBuff += CIDownedBossSystem.DownedLegacyScal ? 5f : 0f;
            return 1f + damageBuff;
        }
    }
}
