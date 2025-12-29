using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class AncientCrusher : CIMelee, ILocalizedModType
    {

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;
            Item.scale = 2f;
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.hammer = 114;
        }

        public override void AddRecipes()
        {

            CreateRecipe().
                AddIngredient(ItemID.Amber, 8).
                AddIngredient(ItemID.FossilOre, 35).
                AddIngredient<EssenceofSunlight>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Vector2.Zero, ProjectileType<FossilSpike>(), hit.Damage, hit.Knockback, Main.myPlayer);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, Vector2.Zero, ProjectileType<FossilSpike>(), hurtInfo.Damage, hurtInfo.Knockback, Main.myPlayer);
        }
    }
}
