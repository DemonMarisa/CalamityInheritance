using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Materials;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Projectiles.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class TerratomereOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.damage = 260;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 21;
            Item.useTurn = true;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.shoot = ModContent.ProjectileType<TerratomereProjectile>();
            Item.shootSpeed = 20f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 4; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-40, 41) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-40, 41) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, (int)(damage * 0.5), knockback, player.whoAmI, 0f, 0f);
            }

            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.TerraBlade);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 30);

            if (!target.canGhostHeal || player.moonLeech)
                return;

            int healAmount = Main.rand.Next(3) + 2;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 30);

            if (player.moonLeech)
                return;

            int healAmount = Main.rand.Next(3) + 2;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Floodtide>().
                AddIngredient<Hellkite>().
                AddIngredient(ItemID.TerraBlade).
                AddIngredient<UelibloomBar>(7).
                AddTile(TileID.LunarCraftingStation).
                Register();

            CreateRecipe().
                AddIngredient<Floodtide>().
                AddIngredient<Hellkite>().
                AddIngredient<TerraEdge>().
                AddIngredient<UelibloomBar>(7).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
