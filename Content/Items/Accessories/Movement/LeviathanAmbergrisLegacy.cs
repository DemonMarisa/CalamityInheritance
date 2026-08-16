using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Projectiles.Typeless.Accessories;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Movement
{
    public class LeviathanAmbergrisLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Movement;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = ItemRarityID.Lime;
            Item.value = CIShopValue.RarityPriceLime;
            Item.defense = 20;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var source = player.GetSource_Accessory(Item);
            player.ignoreWater = true;
            if (!player.lavaWet && !player.honeyWet)
            {
                if (!Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
                {
                    player.endurance += 0.05f;
                    player.GetDamage<GenericDamageClass>() += 0.05f;
                }
                else
                {
                    player.GetDamage<GenericDamageClass>() += 0.1f;
                    player.statDefense += 20;
                    player.moveSpeed += 0.75f;
                }
            }
            if (player.miscCounter % 4 == 0)
            {
                if ((double)player.velocity.X > 0 || (double)player.velocity.Y > 0 || player.velocity.X < -0.1 || player.velocity.Y < -0.1)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int seawaterDamage = (int)player.GetTotalDamage<GenericDamageClass>().ApplyTo(50);
                        Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, 0f, ProjectileType<PoisonousSeawater>(), seawaterDamage, 5f, player.whoAmI, 0f, 0f);
                    }
                }
            }
            Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0f, 0.5f, 1.25f);
            if (player.miscCounter % 25 == 0)
            {
                int damage = player.CalcIntDamage<GenericDamageClass>(30);
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage && Vector2.Distance(player.Center, npc.Center) <= 300f)
                    {
                        if (player.whoAmI == Main.myPlayer)
                        {
                            int dir = npc.Center.X > player.Center.X ? 1 : -1;
                            player.ApplyDamageToNPC(npc, damage, 1f, dir, false, DamageClass.Generic, true);
                        }
                        if (npc.buffImmune[BuffID.Venom])
                            return;
                        if (npc.FindBuffIndex(BuffID.Venom) == -1)
                            npc.AddBuff(BuffID.Venom, 120, false);
                    }
                }
            }
        }
    }
}
