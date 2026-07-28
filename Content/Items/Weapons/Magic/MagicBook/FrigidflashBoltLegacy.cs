using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.MagicBook;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicBook
{
    public class FrigidflashBoltLegacy : CIMagic
    {
        public static readonly SoundStyle UseSound = CISounds.FrigidflashUse;
        public static readonly SoundStyle ProjDeathSound = CISounds.FrigidflashDeath;
        public override void ExSD()
        {
            Item.width = 38;
            Item.height = 42;
            Item.damage = 80;
            Item.mana = 13;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = UseSound;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<FrigidflashBoltProj>();
            Item.shootSpeed = 9f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FrostBoltLegacy>().
                AddIngredient<FlareBoltLegacy>().
                AddIngredient(ItemID.SoulofNight, 2).
                AddIngredient(ItemID.SoulofLight,2).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
