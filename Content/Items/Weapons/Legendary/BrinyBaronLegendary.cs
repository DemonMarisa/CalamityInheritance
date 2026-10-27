using CalamityInheritance.Content.Projectiles.Legendary;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using CalamityMod.Projectiles.Summon;
using Humanizer;
using LAP.Content.Configs;
using LAP.Core.Utilities;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public abstract class LegendaryWeapon : ModItem, ILocalizedModType
    {
        public virtual bool TierOneMatcher => Main.hardMode;
        public virtual bool TierTwoMatcher => Condition.DownedPlantera.IsMet();
        public virtual bool TierThreeMatcher => Condition.DownedMoonLord.IsMet();
        public override string LocalizationCategory => "Content.Items.Weapons.Legendary";
        public virtual int BaseDamage => 75;
        public virtual Color DrawColor => Color.White;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = ItemRarityID.Purple;
            Item.master = true;
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
        public virtual int LegendaryDamage()
        {
            int damage = 0;
            return damage;
        }
    }
    public class BrinyBaronLegendary : LegendaryWeapon
    {
        public float Timer = 0;
        public override int BaseDamage => 75;
        public override Color DrawColor => Color.SkyBlue;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.BonusAttackSpeedMultiplier[Item.type] = 1.33f;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 100;
            Item.height = 102;
            Item.damage = BaseDamage;
            Item.scale *= 1.5f;
            Item.knockBack = 4f;
            Item.useAnimation = Item.useTime = 15;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.shootSpeed = 9f;
            Item.shoot = ProjectileType<BrinyBaronLegendRazor>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
        }
        public override bool? UseItem(Player player)
        {
            bool alt = player.altFunctionUse == 2;
            Item.useTurn = !alt;
            Item.noMelee = alt;
            Item.noUseGraphic = alt;
            Item.UseSound = alt ? SoundID.Item84 : SoundID.Item1;
            return base.UseItem(player);
        }
        public override void HoldItem(Player player)
        {
            if (TierTwoMatcher)
            {
                if (Timer > 0)
                    Timer--;
            }
            if (TierThreeMatcher)
            {
                //10%伤害，10%攻速，5%暴击，1击退，1HP，20防御，10免伤，10移速
                player.GetDamage<MeleeDamageClass>() += 0.10f;
                player.GetAttackSpeed<MeleeDamageClass>() += 0.10f;
                player.GetKnockback<MeleeDamageClass>() += 1;
                player.GetCritChance<MeleeDamageClass>() += 5;
                player.statDefense += 20;
                player.lifeRegen += 2;
                player.endurance += 0.1f;
                player.moveSpeed += 0.1f;
            }
            base.HoldItem(player);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Flare_Blue, 0f, 0f, 100, new Color(53, Main.DiscoG, 255));
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = player.GetSource_ItemUse(Item);
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ProjectileType<BrinyBaronLegendRazor>(), Item.damage, Item.knockBack, player.whoAmI);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            var source = player.GetSource_ItemUse(Item);
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ProjectileType<BrinyBaronLegendBubble>(), Item.damage, Item.knockBack, player.whoAmI);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            bool alt = player.altFunctionUse == 2;
            if (alt)
            {
                damage = (int)(damage * .8f);
                type = ProjectileType<BrinyBaronLegendRazor>();
            }
            else
                type = ProjectileID.None;
        }
         public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 speed = TierOneMatcher ? velocity * 2f : velocity;
                int realDamage = (int)(damage * 0.8f);
                Projectile.NewProjectile(source, position, speed, ProjectileType<BrinyBaronLegendRazor>(), realDamage, knockback, player.whoAmI);
                if (TierTwoMatcher)
                {
                    //遍历最近的NPC
                    NPC target = LAPUtilities.FindClosestTarget(player.Center, 1800f, true);
                    if (target == null)
                        return false;

                    if (Timer == 0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            int j = Main.rand.NextBool() ? -1 : 1;
                            float offset = 800f * j;
                            float posX = player.Center.X + Main.rand.NextFloat(-300f, 300f);
                            //这里得取玩家头顶, 不然敌对单位在玩家底下的时候直接露馅
                            float posY = player.Center.Y - offset;

                            Vector2 pos = new(posX, posY);
                            Vector2 realSpeed = new Vector2(Item.shootSpeed * 3, 0f).RotatedBy(MathHelper.PiOver2 * j);
                            // 直接让其从天上降落。
                            // bro两倍伤害我靠
                            Projectile.NewProjectile(source, pos, realSpeed, ProjectileType<BrinyBaronLegendRazorAlt>(), (int)(damage * 0.25f), knockback, player.whoAmI);
                        }
                        Timer = 0;
                    }
                }
            }
            return false;
        }

    }
}
