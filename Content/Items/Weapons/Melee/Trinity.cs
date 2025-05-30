using CalamityMod;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Trinity : CIMelee, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.damage = 54;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.useTurn = false;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 54;
            Item.value = CIShopValue.RarityPricePink;
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
                    Main.projectile[proj].DamageType = DamageClass.Melee;
                    Main.projectile[proj].penetrate = 1;
                    Main.projectile[proj].extraUpdates = 2;
                }
            }
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.PinkFairy);
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
