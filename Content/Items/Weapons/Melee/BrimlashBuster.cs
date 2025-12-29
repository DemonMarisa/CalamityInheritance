using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class BrimlashBuster: CIMelee, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 72;
            Item.damage = 126;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<BrimlashBusterProj>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Brimlash>().
                AddIngredient<CoreofChaos>(5).
                AddIngredient(ItemID.FragmentSolar, 10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, (int)CalamityDusts.Brimstone);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float damageMult = 0f;
            if (Main.rand.NextBool(3))
                damageMult = 2f;
            damage += damageMult;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<BrimstoneFlames>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffType<BrimstoneFlames>(), 300);
        }
    }
}