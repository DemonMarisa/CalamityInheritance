using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Ranged.Ammo;
using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Ammo.RangedAmmo
{
    public class VeriumBullet : CIAmmo, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Ammo";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 1.25f;
            Item.value = Item.sellPrice(copper: 12);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<VeriumBulletProj>();
            Item.shootSpeed = 16f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).
                AddIngredient(ItemID.MusketBall, 100).
                AddIngredient<CryoBar>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
