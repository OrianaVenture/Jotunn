﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

namespace JotunnDoc.Docs
{
    public class ShaderDoc : Doc
    {
        public ShaderDoc() : base("prefabs/shader-list.md")
        {
            PrefabManager.OnPrefabsRegistered += DocPrefabs;
        }

        private void DocPrefabs()
        {
            if (Generated)
            {
                return;
            }

            Jotunn.Logger.LogInfo("Documenting prefab shaders");

            AddHeader(1, "Shader list");
            AddText("All shaders and their properties currently in the game.");
            AddText("This file is automatically generated from Valheim using the JotunnDoc mod found on our GitHub.");
            AddTableHeader("Shader", "Properties");

            var allPrefabs = ZNetScene.instance.m_prefabs
                .Where(x => ShaderHelper.GetRenderers(x).Any())
                .OrderBy(x => x.name);

            List<Shader> shaders = new List<Shader>();

            foreach (GameObject prefab in allPrefabs)
            {
                foreach (Material mat in ShaderHelper.GetAllRendererMaterials(prefab))
                {
                    // Add distinct shaders
                    if (!shaders.Contains(mat.shader))
                    {
                        shaders.Add(mat.shader);
                    }
                }
            }
            
            foreach (Shader shady in shaders.OrderBy(x => x.name))
            {
                StringBuilder propsb = new StringBuilder();

                if (shady.GetPropertyCount() > 0)
                {
                    propsb.Append("<dl>");
                    for (int i = 0; i < shady.GetPropertyCount(); ++i)
                    {
                        propsb.Append("<dd>");
                        propsb.Append(shady.GetPropertyName(i));
                        string desc = shady.GetPropertyDescription(i);
                        if (!string.IsNullOrEmpty(desc))
                        {
                            propsb.Append($" ({desc})");
                        }
                        propsb.Append("</dd>");
                    }
                    propsb.Append("</dl>");
                }

                AddTableRow(shady.name, propsb.ToString());
            }

            Save();
        }
    }
}