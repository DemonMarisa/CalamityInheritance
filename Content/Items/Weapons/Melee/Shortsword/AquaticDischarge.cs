using CalamityMod.Items;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class AquaticDischarge : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee.Shortsword";
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.DamageType = TrueMeleeDamageClass.Instance;
            Item.useTurn = false;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.width = 32;
            Item.height = 32;
            Item.damage = 23;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<AquaticDischargeProj>();
            Item.shootSpeed = 2f;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.rare = ItemRarityID.Green;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {

        }
    }
}
