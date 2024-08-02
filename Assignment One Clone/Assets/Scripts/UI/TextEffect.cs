using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] float speed = 2.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.ForceMeshUpdate();
        var textArr = text.textInfo;

        for (int i = 0; i < textArr.characterCount; i++)
        {
            var charVar = textArr.characterInfo[i];

            if (!charVar.isVisible) continue;

            var charVertices = textArr.meshInfo[charVar.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                var orig = charVertices[charVar.vertexIndex + j];
                charVertices[charVar.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * speed + orig.x * (0.01f)) * 5f , 0);
            }
        }

        for (int i = 0;i < textArr.meshInfo.Length; i++)
        {
            var meshInfo = textArr.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            text.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
