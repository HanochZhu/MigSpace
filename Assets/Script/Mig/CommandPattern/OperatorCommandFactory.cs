using Mig.Core;
using System;
using UnityEngine;

namespace Mig
{
    public class OperatorCommandFactory
    {
        // TODO Scale and Rotate command

        public static IOperatorCommand CreateColorChangeCommand(MigMaterial material, Color targetColor)
        {
            return new OperatorColorChange(material, targetColor);
        }

        public static IOperatorCommand CreateOperatorMaterialChangeCommand(Renderer renderer, Guid targetMaterialGUID)
        {
            return new OperatorMaterialChange(renderer, targetMaterialGUID);
        }

        public static IOperatorCommand CreateOperatorMetallicChangeCommand(MigMaterial material, float tarfetMetallic)
        {
            return new OperatorMetallicChange(material, tarfetMetallic);
        }

        public static IOperatorCommand CreateOperatorSmoothnessChangeCommand(MigMaterial migMaterial, float tarfetSmoothness)
        {
            return new OperatorSmoothnessChange(migMaterial, tarfetSmoothness);
        }

        public static IOperatorCommand CreateOperatorTransparencyChangeCommand(MigMaterial migMaterial, float tarfetTransparency)
        {
            return new OperatorTransparencyChange(migMaterial, tarfetTransparency);
        }

        public static IOperatorCommand CreateOperatorTilingChangeCommand(MigMaterial migMaterial, Vector2 tarfetTiling)
        {
            return new OperatorTilingChange(migMaterial, tarfetTiling);
        }

        public static IOperatorCommand CreateOperatorOffsetChangeCommand(MigMaterial migMaterial, Vector2 tarfetOffset)
        {
            return new OperatorOffsetChange(migMaterial, tarfetOffset);
        }

        public static IOperatorCommand CreatOperatorMainTextureChangeCommand(Renderer renderer, Texture2D targetTex)
        {
            return new OperatorMainTextureChange(renderer, targetTex);
        }
    }

}

