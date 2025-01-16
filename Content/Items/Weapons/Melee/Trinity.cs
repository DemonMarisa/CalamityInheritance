using CalamityMod;
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
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Projectiles.Pets;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Trinity : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 54;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileID.RubyBolt;
            Item.shootSpeed = 11f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = Utils.SelectRandom(Main.rand, new int[]
            {
                ProjectileID.RubyBolt,
                ProjectileID.SapphireBolt,
                ProjectileID.AmethystBolt
            });
            for (int projectiles = 0; projectiles <= 3; projectiles++)
            {
                float SpeedX = velocity.X + Main.rand.Next(-30, 31) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-30, 31) * 0.05f;
                int proj = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, (int)(damage * 0.6), knockback, Main.myPlayer);
                if (proj.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[proj].CalamityInheritance().forceMelee = true;
                    Main.projectile[proj].penetrate = 1;
                }
            }
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.PinkFairy);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CryonicBar>(9).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
