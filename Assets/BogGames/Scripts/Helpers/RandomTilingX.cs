using UnityEngine;

namespace BogGames.Helpers
{
    public static class MaterialHelper
    {
        public static void SetMainTextureTilingX(Renderer renderer, float xValue)
        {
            if (renderer == null || renderer.material == null)
                return;

            var tiling = renderer.material.mainTextureScale;
            tiling.x = xValue;
            renderer.material.mainTextureScale = tiling;
        }
    }

    public class RandomTilingX : MonoBehaviour
    {
        [SerializeField] private float minTilingX = 0.5f;
        [SerializeField] private float maxTilingX = 2f;

        private void Start()
        {
            var renderer = GetComponent<Renderer>();
            var randomValue = Random.Range(minTilingX, maxTilingX);
            MaterialHelper.SetMainTextureTilingX(renderer, randomValue);
        }
    }
}
