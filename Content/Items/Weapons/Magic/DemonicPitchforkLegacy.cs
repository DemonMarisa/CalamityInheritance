using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Magic;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class DemonicPitchforkLegacy : CIMagic, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 82;
            Item.DamageType= DamageClass.Magic;
            Item.mana = 11;
            Item.width = 56;
            Item.height = 56;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = CISoundID.SoundCurseFlamesAttack;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DemonicPitchforkProjLegacy>();
            Item.shootSpeed = 16f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Obsidian, 20).
                AddIngredient<RottenMatter>(15).
                AddIngredient(ItemID.HellstoneBar, 20).
                AddIngredient(ItemID.SoulofNight, 20).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
