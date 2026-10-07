using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Particles;
using LAP.Content.Particles;
using LAP.Core.Presets.Content;
using LAP.Core.StateMachine.SynedHitEffect;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.HitEffect
{
    public class WeaponTransEffect : BaseHitEffect
    {
        public override void HitEffect(Entity entity, IEntitySource source, Player owner)
        {
            ModItem item = owner?.HeldItem?.ModItem;
            if (item == null || item is not CIMeleeRogue rogue)
                return;
            Color effectColor = rogue.EffectColor;
            new Particles.StrongBloom(owner.Center, Vector2.Zero, effectColor * 0.6f, 0.56f, 9).Spawn();
            new Particles.StrongBloom(owner.Center, Vector2.Zero, effectColor * 0.5f, 0.95f, 12).Spawn();
            new Particles.StrongBloom(owner.Center, Vector2.Zero, Color.White * 0.3f, 1.5f, 14).Spawn();
            new CrossGlow(owner.Center, Vector2.Zero, effectColor, 25, 1f, 0.1f).Spawn();
            new CrossGlow(owner.Center, Vector2.Zero, Color.White, 25, 1f, 0.3f).Spawn();
            for (int i = 0; i < 4; i++)
            {
                Color color = LAPUtilities.LerpColor(effectColor, Color.WhiteSmoke);
                new NoiseShockRing(owner.Center, Vector2.Zero, color, 35, 1f, 0.25f + i * 0.025f, -1, Vector2.Zero, false).Spawn();
            }
            for (int i = 0; i < 25; i++)
            {
                Color RandomColor = LAPUtilities.LerpColor(effectColor, Color.WhiteSmoke);
                ParticlePreset.NewTGlowBall(owner.Center, Vector2.Zero, RandomColor, 55, 0.1f, Main.rand.NextFloat(3f, 6f));
            }
            SoundEngine.PlaySound(CISounds.Switch2 with { Pitch = Main.rand.NextFloat(1f, 2f) }, owner.Center);
        }
    }
}
