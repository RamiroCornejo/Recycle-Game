using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RainbowEffect : TextEffect
{
    Vector3[] vertices;
    Color[] originalColors;

    [SerializeField] Gradient rainbow;

    [SerializeField, Range(0, 3)] float timeMultiplier = 1;

    public override void UpdateEffect(Mesh mesh, TMP_Text text)
    {
        if (!active) return;

        vertices = mesh.vertices;

        Color[] colors = mesh.colors;
        for (int w = 0; w < myWords.Count; w++)
        {
            int wordIndex = myWords[w].index;

            for (int i = 0; i < myWords[w].length; i++)
            {
                TMP_CharacterInfo c = text.textInfo.characterInfo[wordIndex + i];

                int index = c.vertexIndex;

                float time = Time.time * timeMultiplier;

                colors[index] = rainbow.Evaluate(Mathf.Repeat(time + vertices[index].x * 0.001f, 1f));
                colors[index + 1] = rainbow.Evaluate(Mathf.Repeat(time + vertices[index + 1].x * 0.001f, 1f));
                colors[index + 2] = rainbow.Evaluate(Mathf.Repeat(time + vertices[index + 2].x * 0.001f, 1f));
                colors[index + 3] = rainbow.Evaluate(Mathf.Repeat(time + vertices[index + 3].x * 0.001f, 1f));
            }
        }
        mesh.colors = colors;
    }

    public override void ShutDown(Mesh mesh)
    {
        if (!active) return;

        active = false;

        mesh.colors = originalColors;

    }

    public override void OnStartEffect(Mesh mesh)
    {
        originalColors = mesh.colors.Clone() as Color[];
    }
}
