using CalamityMod.Buffs.DamageOverTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Melee.Spear;
using CalamityMod.Projectiles.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Spear
{
    public class FulgurationHalberd : CIMelee, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 114;
            Item.width = 60;
            Item.height = 64;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FulgurationHalberdProj>();
            Item.UseSound = SoundID.Item82;
            Item.shootSpeed = 12f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            Item.noMelee = false;
            Item.noUseGraphic = false;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ProjectileID.None;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.useTurn = false;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.shoot = ModContent.ProjectileType<FulgurationHalberdProj>();
                return base.CanUseItem(player);
            }
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
                Item.useTurn = true;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.shoot = ProjectileID.None;
                return base.CanUseItem(player);
            }
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hit)
        {
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 300);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyAdamantiteBar", 10).
                AddIngredient(ItemID.CrystalShard, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
