using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Magic.MagicBook;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicBook
{
    public class VictideBook : CIMagic
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item17;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<CoralSpikeLegacy>();
            Item.shootSpeed = 16f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AncientVictideBar>(5).
                AddIngredient(ItemID.Coral, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
