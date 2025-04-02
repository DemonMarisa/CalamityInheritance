using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class GlacialCrusher: CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 60;
            Item.rare = ItemRarityID.Pink;
            Item.value = CIShopValue.RarityPricePink;
            Item.useTime = Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = false;
            Item.damage = 125;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.shoot = ModContent.ProjectileType<Iceberg>();
            Item.shootSpeed = 12f;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if(Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.IceRod);
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if(target.Calamity().gState >= 0)
            {
                modifiers.ModifyHitInfo += (ref NPC.HitInfo hitnfo) =>
                {
                    hitnfo.Damage *= 2;
                    hitnfo.Knockback *= 3f;
                };
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(target.Calamity().gState>=0)
            {
                SoundEngine.PlaySound(SoundID.NPCHit3);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
        }
    }
}