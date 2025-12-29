using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Summon
{
    public class DazzlingStabberStaffLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Summon;
        public override void ExSD()
        {
            Item.width = 54;
            Item.height = 52;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.DD2_DarkMageHealImpact;
            Item.mana = 10;
            Item.damage = 127;
            Item.knockBack = 2f;
            Item.autoReuse = true;
            Item.useTime = Item.useAnimation = 15;
            Item.shoot = ProjectileType<DazzlingStabberProj>();
            Item.shootSpeed = 13f;

            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                int p = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
                if (Main.projectile.IndexInRange(p))
                    Main.projectile[p].originalDamage = Item.damage;
            }
            float angleMax = MathHelper.ToRadians(45f);
            if (CalamityUtils.CountProjectiles(type) == 1)
                angleMax = 0f;
            float index = 1f;
            if (player.ownedProjectileCounts[Item.shoot] > 8)
            {
                angleMax += MathHelper.ToRadians((player.ownedProjectileCounts[Item.shoot] - 8) * 2.5f);
            }
            angleMax = angleMax > MathHelper.ToRadians(105f) ? MathHelper.ToRadians(105f) : angleMax; // More intuative than using a min function
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type == type && p.owner == player.whoAmI)
                {
                    p.ai[1] = (index / CalamityUtils.CountProjectiles(type)) * angleMax - angleMax / 2f;
                    p.netUpdate = true;
                    index++;
                }
            }
            return false;
        }
    }
}
