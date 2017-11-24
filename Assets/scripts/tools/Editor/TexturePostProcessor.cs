// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;
using UnityEditor;

/// <summary>
/// Change default image import settings
/// </summary>
public class TexturePostProcessor : AssetPostprocessor
{
  void OnPostprocessTexture(Texture2D texture)
  {
    TextureImporter importer = assetImporter as TextureImporter;

    if (importer.assetPath.ToLower().Contains("sprites/"))
    {
      importer.textureType = TextureImporterType.Sprite;
      importer.mipmapEnabled = false;
      importer.spritePixelsPerUnit = 25;
      Apply(importer, FilterMode.Point, TextureImporterCompression.Uncompressed);
    }
  }
  
  private void Apply(TextureImporter importer, FilterMode filterMode, TextureImporterCompression compression)
  {
    if (importer.filterMode != filterMode || importer.textureCompression != compression)
    {
      importer.filterMode = filterMode;
      importer.textureCompression = compression;
      importer.alphaIsTransparency = true;
      importer.wrapMode = TextureWrapMode.Repeat;
      importer.mipmapEnabled = false;

      // Reimport.
      AssetDatabase.ImportAsset(importer.assetPath);
    }
  }
}