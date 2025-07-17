using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;

namespace CalamityInheritance.Core
{
    /// <summary>
    /// 从XNA的VertexPositionColorTexture复制来的，用于适配tr的三维材质
    /// 具体改动内容可以查看https://www.bilibili.com/video/BV1zQc8eCE95/?spm_id_from=333.1387.favlist.content.click&vd_source=c253157e780954c2bbfb555b8dff2ada
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CIVertexPositionColorTexture : IVertexType
    {
        #region Private Properties

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexDeclaration;
            }
        }

        #endregion

        #region Public Variables

        public Vector2 Position;
        public Color Color;
        public Vector3 TextureCoordinate;

        #endregion

        #region Public Static Variables

        public static readonly VertexDeclaration VertexDeclaration;

        #endregion

        #region Private Static Constructor

        static CIVertexPositionColorTexture()
        {
            VertexDeclaration = new VertexDeclaration(
                new VertexElement[]
                {
                    new VertexElement(
                        0,
                        VertexElementFormat.Vector2,
                        VertexElementUsage.Position,
                        0
                    ),
                    new VertexElement(
                        8,
                        VertexElementFormat.Color,
                        VertexElementUsage.Color,
                        0
                    ),
                    new VertexElement(
                        12,
                        VertexElementFormat.Vector3,
                        VertexElementUsage.TextureCoordinate,
                        0
                    )
                }
            );
        }

        #endregion

        #region Public Constructor

        public CIVertexPositionColorTexture(
            Vector2 position,
            Color color,
            Vector3 textureCoordinate
        )
        {
            Position = position;
            Color = color;
            TextureCoordinate = textureCoordinate;
        }

        #endregion

        #region Public Static Operators and Override Methods

        public override int GetHashCode()
        {
            // TODO: Fix GetHashCode
            return 0;
        }

        public override string ToString()
        {
            return (
                "{{Position:" + Position.ToString() +
                " Color:" + Color.ToString() +
                " TextureCoordinate:" + TextureCoordinate.ToString() +
                "}}"
            );
        }

        public static bool operator ==(CIVertexPositionColorTexture left, CIVertexPositionColorTexture right)
        {
            return ((left.Position == right.Position) &&
                    (left.Color == right.Color) &&
                    (left.TextureCoordinate == right.TextureCoordinate));
        }

        public static bool operator !=(CIVertexPositionColorTexture left, CIVertexPositionColorTexture right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != base.GetType())
            {
                return false;
            }

            return (this == ((CIVertexPositionColorTexture)obj));
        }

        #endregion
    }
}
