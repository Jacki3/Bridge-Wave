using UnityEngine;
using UnityEngine.UI;

namespace BogGames.UI
{
    [RequireComponent(typeof(Image))]
    public class GradientFill : BaseMeshEffect
    {
        public Color color1 = new Color(1, 1, 1, 0); // Transparent
        public Color color2 = Color.red;             // Bold color
        public bool horizontal = true;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            Rect rect = ((RectTransform)transform).rect;
            UIVertex vertex = new UIVertex();

            int count = vh.currentVertCount;
            if (count == 0) return;

            // Track min/max depending on orientation
            float min = float.MaxValue;
            float max = float.MinValue;

            for (int i = 0; i < count; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);
                float pos = horizontal ? vertex.position.x : vertex.position.y;
                if (pos < min) min = pos;
                if (pos > max) max = pos;
            }

            float width = max - min;

            for (int i = 0; i < count; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);
                float pos = horizontal ? vertex.position.x : vertex.position.y;
                float t = Mathf.InverseLerp(min, max, pos); // normalize within visible rect
                vertex.color = Color.Lerp(color1, color2, t);
                vh.SetUIVertex(vertex, i);
            }
        }
    }
}