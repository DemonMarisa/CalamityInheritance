using CalamityInheritance.Buffs.StatDebuffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Waraxe : CIMelee, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.knockBack = 5.25f;
            Item.useTime = 18;
            Item.useAnimation = 22;
            Item.axe = 85 / 5;

            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 40;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                target.AddBuff(ModContent.BuffType<WarCleave>(), 900);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hit)
        {
            target.AddBuff(ModContent.BuffType<WarCleave>(), 900);
        }
    }
}
