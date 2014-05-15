using UnityEngine;
using System.Collections;

public class RandomTextureSetter : MonoBehaviour
{
    [SerializeField]
    private Texture[] _textures;

    private void Start()
    {
        if (Application.loadedLevelName==Consts.SceneNames.Level1.ToString())
            renderer.material.mainTexture = RandomUtils.GetRandomItem(_textures);
    }

}
