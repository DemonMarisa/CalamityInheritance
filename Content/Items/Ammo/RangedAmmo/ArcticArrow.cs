using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Ammo;
using CalamityInheritance.Content.Projectiles.Ranged.Ammo;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Ammo.RangedAmmo
{
    public class ArcticArrow : CIAmmo, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Ammo";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 36;
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileType<ArcticArrowProj>();
            Item.shootSpeed = 13f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            CreateRecipe(250).
                AddIngredient<CryoBar>().
                AddTile(TileID.IceMachine).
                Register();
        }
    }
}
