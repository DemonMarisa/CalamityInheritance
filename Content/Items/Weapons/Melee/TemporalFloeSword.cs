using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Projectiles.Pets;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityInheritance.Content.Items.Materials;
using CalamityMod.Items.Placeables.SunkenSea;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class TemporalFloeSword : CIMelee, ILocalizedModType
    {

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.damage = 95;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 16;
            Item.knockBack = 6;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 60;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ProjectileType<TemporalFloeSwordProjectile>();
            Item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, (int)(damage * 0.65), knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CryoBar>(15)
                .AddIngredient<SeaPrism>(15)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddTile(TileID.IceMachine)
                .Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 34);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<GlacialState>(), 60);
            target.AddBuff(BuffID.Frostburn, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffType<GlacialState>(), 60);
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }
}
