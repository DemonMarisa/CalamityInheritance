using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Legendary;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityMod;
using Humanizer;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    //yo wtf.
    public class MalachiteLegendary : CIRogue
    {
        public bool TierOneMatcher => Main.hardMode;
        public bool TierTwoMatcher => Condition.DownedPlantera.IsMet();
        public bool TierThreeMatcher => Condition.DownedMoonLord.IsMet();
        public new string LocalizationCategory => "Content.Items.Weapons.Legendary";
        public int BaseDamage => 75;
        public static readonly SoundStyle StealthSound = new("CalamityMod/Sounds/Item/WulfrumKnifeThrowSingle") { PitchVariance = 0.4f };
        public Color DrawColor => new(0, 255, 127);
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override void ExSD()
        {
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = ItemRarityID.Purple;
            Item.master = true;
            Item.width = 26;
            Item.height = 58;
            Item.damage = BaseDamage;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1.25f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<MalachiteLegendProj>();
            Item.shootSpeed = 10f;
            Item.LAP().SkillShoot = ProjectileType<MalachiteLegendBeam>();
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(BaseDamage + LegendaryDamage()) / (float)BaseDamage);
            damage *= Buff;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            if (line.Mod == "Terraria" && line.Name == "ItemName")
            {
                float sine = (float)((1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2.5f)) / 2);
                float sineOffset = MathHelper.Lerp(0.8f, 1f, sine);
                string textValue = line.Text;
                Vector2 textPos = new(line.X, line.Y);
                //绘制发光描边
                for (int i = 0; i < 12; i++)
                {
                    Vector2 afterimageOffset = (MathHelper.TwoPi * i / 12f).ToRotationVector2() * (1.8f * sineOffset);
                    ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, textValue, (textPos + afterimageOffset).RotatedBy(MathHelper.TwoPi * (i / 12)), Color.Black, line.Rotation, line.Origin, line.BaseScale);
                }
                ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, textValue, textPos, DrawColor, line.Rotation, line.Origin, line.BaseScale);
            }
            if (line.Mod != Mod.Name)
                return;
            conditionMatch(TierOneMatcher, 1);
            conditionMatch(TierTwoMatcher, 2);
            conditionMatch(TierThreeMatcher, 3);
            void conditionMatch(bool condition, int index)
            {
                Color grayColor = Color.LightGray;
                if (line.Name == "Tier" + index + "Name")
                {
                    string textValue = line.Text;
                    Vector2 textPos = new(line.X, line.Y);
                    ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, textValue, textPos,
                       condition ? DrawColor : grayColor, line.Rotation, line.Origin, line.BaseScale);
                }
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string t1TextValue = this.GetLocalizedValue("Tier1");
            string t2TextValue = this.GetLocalizedValue("Tier2");
            string t3TextValue = this.GetLocalizedValue("Tier3");
            string levelTextValue = Language.GetTextValue(Mod.GetLocalizationKey("Content.Items.Weapons.Legendary.LevelName"));
            string levelTextValueFormat = levelTextValue.FormatWith("+" + LegendaryDamage());
            if (Main.keyState.PressingAlt())
            {
                if (tooltips.Count > 2)
                {
                    tooltips.RemoveRange(1, tooltips.Count - 1);
                    TooltipLine line1 = new TooltipLine(Mod, "Tier1Name", t1TextValue);
                    TooltipLine line2 = new TooltipLine(Mod, "Tier2Name", t2TextValue);
                    TooltipLine line3 = new TooltipLine(Mod, "Tier3Name", t3TextValue);
                    TooltipLine lineLevel = new TooltipLine(Mod, "LevelName", levelTextValueFormat);
                    //先后加进来的顺序
                    tooltips.Insert(1, lineLevel);
                    tooltips.Insert(1, line3);
                    tooltips.Insert(1, line2);
                    tooltips.Insert(1, line1);
                }
            }
        }
        /// <summary>
        /// 复写这个以实现伤害加成
        /// </summary>
        /// <returns></returns>
        public int LegendaryDamage()
        {
            int damage = 0;
            return damage;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = Item.useAnimation = 8;
                Item.UseSound = SoundID.Item109;
            }
            else
            {
                Item.useTime = Item.useAnimation = 4;
                Item.UseSound = CISoundID.SoundWeaponSwing;
            }
            return true;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int pNum = TierOneMatcher ? 6 : 5;
            int dmg = (int)(damage * 0.7f);
            for (int j = 1; j <= pNum; j++)
            {
                Vector2 spread = new Vector2(velocity.X, velocity.Y).RotatedBy(j / 11f + 0.2f) * j / 5f;
                int p = Projectile.NewProjectile(source, position + velocity * 0.1f, velocity + spread, Item.LAP().SkillShoot, dmg, knockback, player.whoAmI, 0f, 0f, -1f);
                Main.projectile[p].SetStealthAttack();
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
                Projectile.NewProjectile(source, position, velocity, ProjectileType<MalachiteLegendBolt>(), (int)(damage * 1.75f), knockback, player.whoAmI);
            else
                Projectile.NewProjectile(source, position, velocity, type, (int)(damage * 1.75f), knockback, player.whoAmI);
            return false;
        }
    }
}