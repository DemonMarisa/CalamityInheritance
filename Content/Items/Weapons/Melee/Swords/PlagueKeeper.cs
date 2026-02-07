using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class PlagueKeeper : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 90;
            Item.damage = 68;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useTurn = false;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ProjectileType<VirulentBeeWave>();
            Item.shootSpeed = 9f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = player.GetSource_ItemUse(Item);

            target.AddBuff(ModContent.BuffType<Plague>(), 300);
            for (int i = 0; i < 3; i++)
            {
                int bee = Projectile.NewProjectile(source, player.Center, Vector2.Zero, player.beeType(),
                    player.beeDamage(Item.damage / 3), player.beeKB(0f), player.whoAmI);
                if (bee.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[bee].penetrate = 1;
                    Main.projectile[bee].DamageType = DamageClass.Melee;
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            var source = player.GetSource_ItemUse(Item);

            target.AddBuff(ModContent.BuffType<Plague>(), 300);
            for (int i = 0; i < 3; i++)
            {
                int bee = Projectile.NewProjectile(source, player.Center, Vector2.Zero, player.beeType(),
                    player.beeDamage(Item.damage / 3), player.beeKB(0f), player.whoAmI);
                if (bee.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[bee].penetrate = 1;
                    Main.projectile[bee].DamageType = DamageClass.Melee;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Virulence>().
                AddIngredient<InfectedArmorPlating>(5).
                AddIngredient(ItemID.BeeKeeper).
                AddIngredient(ItemID.FragmentSolar, 5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
