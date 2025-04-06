using System.Collections.Generic;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    //我本人完全看不懂这个代码，如果需要的话可能重写？
    public class CyrogenLegendary: CISummon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Summon";
        public static string TextRoute => $"{Generic.GetWeaponLocal}.Summon.CyrogenLegendary";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 50;
            Item.damage = 48;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item30;
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<CryogenPtr>();
            Item.DamageType = DamageClass.Summon;
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = CIConfig.Instance.LegendaryRarity ? ModContent.RarityType<CryogenBlue>() : ModContent.RarityType<MaliceChallengeDrop>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            string t1 = mp.ColdDivityTier1? Language.GetTextValue($"{TextRoute}.TierOne") : Language.GetTextValue($"{TextRoute}.TierOneTint");
            tooltips.FindAndReplace("[TIERONE]", t1);
            string t2 = mp.ColdDivityTier2? Language.GetTextValue($"{TextRoute}.TierTwo") : Language.GetTextValue($"{TextRoute}.TierTwoTint");
            tooltips.FindAndReplace("[TIERTWO]", t2);
            string t3 = mp.ColdDivityTier3? Language.GetTextValue($"{TextRoute}.TierThree") : Language.GetTextValue($"{TextRoute}.TierThreeTint");
            tooltips.FindAndReplace("[TIERTHREE]", t3);
            //以下，用于比较复杂的计算
            float getDmg = LegendaryDamage();
            int boostPercent = (int)(getDmg * 100);
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
        }

        private float LegendaryDamage()
        {
            float newDamage = 0f;
            newDamage += DownedBossSystem.downedRavager ? 0.2f : 0f;
            newDamage += Condition.DownedEmpressOfLight.IsMet() ? 0.2f : 0f;
            newDamage += Condition.DownedDukeFishron.IsMet() ? 0.2f : 0f;
            newDamage += Condition.DownedCultist.IsMet() ? 0.4f : 0f;
            newDamage += Condition.DownedMoonLord.IsMet() ? 0.4f : 0f;
            newDamage += DownedBossSystem.downedGuardians ? 0.4f : 0f;
            newDamage += DownedBossSystem.downedProvidence  ? 0.6f : 0f;
            newDamage += DownedBossSystem.downedPolterghast ? 0.6f : 0f;
            newDamage += DownedBossSystem.downedDoG ? 0.8f : 0f;
            newDamage += DownedBossSystem.downedYharon ? 1.0f : 0f;
            newDamage += DownedBossSystem.downedExoMechs || DownedBossSystem.downedCalamitas ? 1.5f : 0f;
            return 1f + newDamage;
        }

        public override bool AltFunctionUse(Player player)
        {
            return base.AltFunctionUse(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //搜寻世界上存在可能的由本人提供的召唤物
            float allMinions = 0f;
            foreach(Projectile pro in Main.ActiveProjectiles)
            {
                //如果是本人的召唤物，把这个召唤物的栏位需求赋值
                if (pro.minion && pro.owner == player.whoAmI)
                    allMinions += pro.minionSlots;
            }
            //用剩下的召唤栏召唤左键的召唤物 
            if (player.altFunctionUse != 2 && allMinions < player.maxMinions)
            {
                //给自己发送一个buff
                player.AddBuff(ModContent.BuffType<CyrogenLegendaryBuff>(), 120, true);
                //将鼠标位置赋值
                position = Main.MouseWorld;
                int p = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
                //实时刷新伤害
                if (Main.projectile.IndexInRange(p))
                    Main.projectile[p].originalDamage = Item.damage;
                
                //查询当前在玩家身上持有的射弹数量
                int ptr = 0;
                foreach (Projectile pro in Main.ActiveProjectiles)
                {
                    if (pro.type == type && pro.owner == player.whoAmI)
                    {
                        if(!(pro.ModProjectile as CryogenPtr).Idle)
                            continue;
                        ptr++;
                    }
                }
                float varAngle = MathHelper.TwoPi / ptr;
                float angle = 0f;
                foreach (Projectile pro in Main.ActiveProjectiles)
                {
                    if (pro.type == type && pro.owner == player.whoAmI && pro.ai[1] == 0f)
                    {
                        if (!(pro.ModProjectile as CryogenPtr).Idle)
                            continue;
                        pro.ai[0] = angle;
                        pro.netUpdate = true;
                        angle += varAngle;
                        for (int j = 0; j < 22; j++)
                        {
                            Dust d = Dust.NewDustDirect(pro.position, pro.width, pro.height, DustID.Ice);
                            d.velocity = Vector2.UnitY * Main.rand.NextFloat(3f, 5.5f) * Main.rand.NextBool().ToDirectionInt();
                            d.noGravity = true;
                        }
                    }
                }
            }
            return false;
        }
    }
}