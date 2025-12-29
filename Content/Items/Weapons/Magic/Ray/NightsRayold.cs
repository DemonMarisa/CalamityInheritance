using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Magic.Ray;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Ray
{
    public class NightsRayold : CIMagic, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<NightsRayBeamold>();
            Item.shootSpeed = 6f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(0, -2);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Vilethorn).
                AddIngredient(ItemID.MagicMissile).
                AddIngredient(ItemID.WandofSparking).
                AddIngredient(ItemID.ThunderStaff).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
