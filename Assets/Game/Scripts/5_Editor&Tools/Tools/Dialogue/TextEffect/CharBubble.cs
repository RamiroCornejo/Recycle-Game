using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharBubble : TextEffect
{

    Vector3[] vertices;
    Vector3[] originalVertices;

    [SerializeField, Range(0, 20)] float timeMultiplier = 1;
    [SerializeField, Range(0, 7)] float potency = 1;

    public override void OnStartEffect(Mesh mesh)
    {
        originalVertices = mesh.vertices.Clone() as Vector3[];
    }

    public override void ShutDown(Mesh mesh)
    {
        if (!active) return;

        active = false;

        mesh.vertices = originalVertices;
    }

    public override void UpdateEffect(Mesh mesh, TMP_Text text)
    {
        if (!active) return;

        vertices = mesh.vertices;

        for (int w = 0; w < myWords.Count; w++)
        {
            int wordIndex = myWords[w].index;

            for (int i = 0; i < myWords[w].length; i++)
            {
                TMP_CharacterInfo c = text.textInfo.characterInfo[wordIndex + i];

                int index = c.vertexIndex;

                Vector3 offset = Wobble(Time.time * timeMultiplier + i);
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }
        }

        mesh.vertices = vertices;
        text.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2.5f)) * potency;
    }
}
