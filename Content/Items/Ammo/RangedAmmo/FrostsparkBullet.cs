using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Projectiles.Pets;
using CalamityInheritance.Content.Projectiles.Ammo;

namespace CalamityInheritance.Content.Items.Ammo.RangedAmmo
{
    public class FrostsparkBullet : CIAmmo, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Ammo";
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1.25f;
            Item.value = 600;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<FrostsparkBulletProj>();
            Item.shootSpeed = 14f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            CreateRecipe(150).
                AddIngredient(ItemID.MusketBall, 150).
                AddRecipeGroup(CIRecipeGroup.CryoBar).
                AddTile(TileID.IceMachine).
                Register();
        }
    }
}
