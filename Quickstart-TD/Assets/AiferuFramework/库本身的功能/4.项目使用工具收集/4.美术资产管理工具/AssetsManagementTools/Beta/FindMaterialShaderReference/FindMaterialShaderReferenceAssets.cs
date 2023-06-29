using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FindMaterialShaderReferenceAssets", menuName = "FindFindMaterialShaderReference/FindMaterialShaderReferenceAssets", order = 4)]
public class FindMaterialShaderReferenceAssets : ScriptableObject
{
   public Shader shader;
   public List<Material> ShaderReferenceMaterials = new List<Material>();
}
