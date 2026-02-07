using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityMod.Items;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class BalefulHarvesterLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;

        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 86;
            Item.damage = 205;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 3; i++)
            {
                int type = Main.rand.NextBool() ? ProjectileType<BalefulHarvesterProjectileLegacy>() : ProjectileID.FlamingJack;
                Vector2 firePos = target.Center + Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 1080;
                Vector2 Vel = LAPUtilities.GetVector2(firePos, target.Center) * 12;
                int damage = player.GetWeaponDamage(Item);
                Projectile.NewProjectile(Item.GetSource_FromThis(), firePos, Vel, type, damage, Item.knockBack, player.whoAmI);
            }
            target.AddBuff(BuffID.OnFire3, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TheHorsemansBlade).
                AddIngredient(ItemID.FragmentStardust, 12).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
