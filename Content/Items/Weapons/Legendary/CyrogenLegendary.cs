using System.Collections.Generic;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;
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
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Summon";
        public static string TextRoute => $"{Generic.WeaponTextPath}Summon.CyrogenLegendary";
        public static readonly float ShootSpeed = 10f;
        public static int baseDamage = 48;
        public static int TrueDamage = 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 50;
            Item.damage = baseDamage;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item30;
            Item.autoReuse = true;
            Item.shootSpeed = ShootSpeed;
            Item.shoot = ModContent.ProjectileType<CryogenPtr>();
            Item.DamageType = DamageClass.Summon;
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = CIConfig.Instance.LegendaryRarity ? ModContent.RarityType<CryogenBlue>() : ModContent.RarityType<MaliceChallengeDrop>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            var tiers = new List<(string placehloder, bool isEnable, string textKey)>
            {
                ("[TIERONE]",mp.ColdDivityTier1,"TierOne"),
                ("[TIERTWO]",mp.ColdDivityTier2,"TierTwo"),
                ("[TIERTHREE]",mp.ColdDivityTier3,"TierThree")
            };
            static void ReplaceTierTooltip(List<TooltipLine> tooltips,string placeholder,bool isEnable,string textKey)
            {
                string text = isEnable ? Language.GetTextValue($"{TextRoute}.{textKey}") : Language.GetTextValue($"{TextRoute}.{textKey}Tint");
                tooltips.FindAndReplace(placeholder, text);
            }
            foreach (var (placehloder, isEnable, textKey) in tiers)
                ReplaceTierTooltip(tooltips, placehloder, isEnable, textKey);
            
            
            //用于发送传奇武器在至尊灾厄眼在场时得到数值增强的信息
            string t4 = null;
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                t4 = Language.GetTextValue($"{Generic.WeaponTextPath}EmpoweredTooltip.Generic");
            //以下，用于比较复杂的计算
            float getDmg = LegendaryDamage();
            string update = this.GetLocalization("LegendaryScaling").Format(
                getDmg.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
            if (t4 != null)
                tooltips.Add(new TooltipLine(Mod, "Buff", t4));
        } 
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(baseDamage + LegendaryDamage() + Generic.GenericLegendBuffInt()) / (float)baseDamage);
            damage *= Buff;
        }
        public static int LegendaryDamage()
        {
            int dmgBuff = 0;
            bool DownCalclone = DownedBossSystem.downedCalamitasClone || CIDownedBossSystem.DownedCalClone;
            dmgBuff += DownCalclone ? 2 : 0;                           // 50
            dmgBuff += Condition.DownedPlantera.IsMet() ? 2 : 0;       // 52
            dmgBuff += DownedBossSystem.downedLeviathan ? 3 : 0;       // 55
            dmgBuff += DownedBossSystem.downedAstrumAureus ? 3 : 0;    // 58
            dmgBuff += Condition.DownedGolem.IsMet() ? 3 : 0;          // 61
            dmgBuff += Condition.DownedEmpressOfLight.IsMet() ? 3 : 0; // 64
            dmgBuff += Condition.DownedDukeFishron.IsMet() ? 3 : 0;    // 67
            dmgBuff += DownedBossSystem.downedRavager ? 3 : 0;         // 70
            dmgBuff += DownedBossSystem.downedPlaguebringer ? 3 : 0;   // 73
            dmgBuff += Condition.DownedCultist.IsMet() ? 7 : 0;        // 80
            dmgBuff += DownedBossSystem.downedAstrumDeus ? 15 : 0;     // 95
            dmgBuff += Condition.DownedMoonLord.IsMet() ? 35 : 0;      // 130
            dmgBuff += DownedBossSystem.downedGuardians ? 10 : 0;      // 140
            dmgBuff += DownedBossSystem.downedDragonfolly ? 10 : 0;    // 150
            dmgBuff += DownedBossSystem.downedProvidence ? 150 : 0;    // 300
            dmgBuff += DownedBossSystem.downedSignus ? 15 : 0;         // 315
            dmgBuff += DownedBossSystem.downedCeaselessVoid ? 15 : 0;  // 330
            dmgBuff += DownedBossSystem.downedStormWeaver ? 15 : 0;    // 345
            dmgBuff += DownedBossSystem.downedPolterghast ? 80 : 0;    // 425
            dmgBuff += DownedBossSystem.downedBoomerDuke ? 25 : 0;     // 450
            dmgBuff += DownedBossSystem.downedDoG ? 350 : 0;           // 800
            dmgBuff += DownedBossSystem.downedYharon ? 700 : 0;        // 1300
            dmgBuff += DownedBossSystem.downedCalamitas ? 100 : 0;     // 1400
            dmgBuff += DownedBossSystem.downedExoMechs ? 100 : 0;      // 1500
            dmgBuff += DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas && DownedBossSystem.downedPrimordialWyrm && CIDownedBossSystem.DownedLegacyScal ? 3500 : 0;
            return dmgBuff;
        }

        public override bool AltFunctionUse(Player player)
        {
            return base.AltFunctionUse(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            TrueDamage = damage;
            //搜寻世界上存在可能的由本人提供的召唤物
            float allMinions = 0f;
            foreach (Projectile pro in Main.ActiveProjectiles)
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
                //if (Main.projectile.IndexInRange(p))
                //    Main.projectile[p].originalDamage = damage;

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