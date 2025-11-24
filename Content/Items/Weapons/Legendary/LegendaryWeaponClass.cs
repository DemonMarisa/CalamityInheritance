using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public abstract class LegendaryWeaponClass : ModItem, ILocalizedModType
    {
        public enum ClassType
        {
            Melee,
            Ranged,
            Magic,
            Summon,
            Rogue
        }
        public virtual ClassType WeaponDamageClass { get; }
        public virtual int SetRarityColor { get; }
        public virtual Color DrawColor { get; }
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.{WeaponDamageClass}";
        public string GeneralLegendItemTextPath => Generic.WeaponTextPath + WeaponDamageClass + "." + GetType().Name;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            ExSSD();
        }
        public virtual void ExSSD() { }
        public override void SetDefaults()
        {
            Item.DamageType = GetNeedDamageClass;
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = CIConfig.Instance.SpecialRarityColor ? SetRarityColor : ModContent.RarityType<MaliceChallengeDrop>();
            ExSD();
        }

        public virtual void ExSD() { }
        public override bool AltFunctionUse(Player player) => true;
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = GetNeedItemGroup;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            //获取全局Sin值，让描边发生一定程度的动态变化
            //除非你的武器不存在武器名。否则这个判定不可能过不去
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
                return false;
            }
            return base.PreDrawTooltipLine(line, ref yOffset);
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = TextureAssets.Item[Type].Value;
            Vector2 position = Item.position - Main.screenPosition + tex.Size() / 2;
            Rectangle iFrame = tex.Frame();
            //添加描边
            for (int i = 0; i < 16; i++)
                spriteBatch.Draw(tex, position + MathHelper.ToRadians(i * 60f).ToRotationVector2() * 1.6f, null, Color.White with { A = 0 }, 0f, tex.Size() / 2, scale, 0, 0f);
            //绘制武器本身
            spriteBatch.Draw(tex, position, iFrame, Color.White, 0f, tex.Size() / 2, scale, 0f, 0f);
            Lighting.AddLight(position, TorchID.UltraBright);
            return false;
        }
        private DamageClass GetNeedDamageClass
        {
            get
            {
                return WeaponDamageClass switch
                {
                    ClassType.Melee => DamageClass.Melee,
                    ClassType.Ranged => DamageClass.Ranged,
                    ClassType.Magic => DamageClass.Magic,
                    ClassType.Summon => DamageClass.Summon,
                    ClassType.Rogue => ModContent.GetInstance<RogueDamageClass>(),
                    _ => DamageClass.Generic,
                };
            }
        }
        private ContentSamples.CreativeHelper.ItemGroup GetNeedItemGroup
        {
            get
            {
                return WeaponDamageClass switch
                {
                    ClassType.Melee => ContentSamples.CreativeHelper.ItemGroup.MeleeWeapon,
                    ClassType.Ranged => ContentSamples.CreativeHelper.ItemGroup.RangedWeapon,
                    ClassType.Magic => ContentSamples.CreativeHelper.ItemGroup.MagicWeapon,
                    ClassType.Summon => ContentSamples.CreativeHelper.ItemGroup.SummonWeapon,
                    _ => ContentSamples.CreativeHelper.ItemGroup.RemainingUseItems
                };
            }
        }
    }
}
