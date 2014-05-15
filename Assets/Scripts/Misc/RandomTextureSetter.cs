using UnityEngine;
using System.Collections;

public class RandomTextureSetter : MonoBehaviour
{
    [SerializeField]
    private Texture[] _textures;

    private void Start()
    {
        renderer.material.mainTexture = RandomUtils.GetRandomItem(_textures);
    }

}
