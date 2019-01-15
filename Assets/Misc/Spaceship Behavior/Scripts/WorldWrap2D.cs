using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.Mechanics
{
    public class WorldWrap2D : MonoBehaviour
    {

        public Rect worldBounds = new Rect(-40, -40, 80, 80);
        public bool enableNineGhost = true;

        private Sprite ghostSprite;

        private GameObject anchor;
        private Transform[] ghostTransforms;

        // Use this for initialization
        void Start()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Sprite ghostSprite = spriteRenderer.sprite;
            Color ghostColor = spriteRenderer.color;

            if (enableNineGhost)
            {
                ghostTransforms = new Transform[8];

                anchor = new GameObject(name + "_Ghost_Anchor");
                anchor.transform.position = transform.position;

                Vector3[] neighborhood = GetNeighborhood(transform.position, worldBounds);

                for (int i = 0; i < neighborhood.Length; i++)
                {
                    GameObject ghost = new GameObject(name + "_Ghost");
                    var sr = ghost.AddComponent<SpriteRenderer>();
                    sr.sprite = ghostSprite;
                    sr.color = ghostColor;
                    ghost.transform.SetParent(anchor.transform);
                    ghost.transform.localScale = transform.localScale;
                    ghost.transform.position = neighborhood[i];

                    ghostTransforms[i] = ghost.transform;
                }
            }
        }

        private void OnDisable()
        {
            if (enableNineGhost)
                Destroy(anchor);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(new Vector3(worldBounds.xMin, worldBounds.yMin), new Vector3(worldBounds.xMin, worldBounds.yMax));
            Gizmos.DrawLine(new Vector3(worldBounds.xMin, worldBounds.yMin), new Vector3(worldBounds.xMax, worldBounds.yMin));
            Gizmos.DrawLine(new Vector3(worldBounds.xMax, worldBounds.yMin), new Vector3(worldBounds.xMax, worldBounds.yMax));
            Gizmos.DrawLine(new Vector3(worldBounds.xMin, worldBounds.yMax), new Vector3(worldBounds.xMax, worldBounds.yMax));

        }

        // Update is called once per frame
        void Update()
        {
            Wrap();
            if (enableNineGhost && anchor)
                UpdateGhostsTransforms();
        }

        private void Wrap()
        {
            if (transform.position.x > worldBounds.xMax)
                transform.position = new Vector3(worldBounds.xMin, transform.position.y);

            if (transform.position.x < worldBounds.xMin)
                transform.position = new Vector3(worldBounds.xMax, transform.position.y);

            if (transform.position.y > worldBounds.yMax)
                transform.position = new Vector3(transform.position.x, worldBounds.yMin);

            if (transform.position.y < worldBounds.yMin)
                transform.position = new Vector3(transform.position.x, worldBounds.yMax);
        }

        private void UpdateGhostsTransforms()
        {
            anchor.transform.position = transform.position;

            for (int i = 0; i < ghostTransforms.Length; i++)
            {
                ghostTransforms[i].localRotation = transform.rotation;
            }
        }

        Vector3[] GetNeighborhood(Vector3 position, Rect size)
        {
            Vector3[] hood = new Vector3[8];

            // Top Left
            hood[0] = new Vector3(position.x - size.width, position.y + size.height, 0);
            // Top
            hood[1] = new Vector3(position.x, position.y + size.height, 0);
            // Top Right
            hood[2] = new Vector3(position.x + size.width, position.y + size.height, 0);
            // Left
            hood[3] = new Vector3(position.x - size.width, position.y, 0);
            // Right
            hood[4] = new Vector3(position.x + size.width, position.y, 0);
            // Bottom Left
            hood[5] = new Vector3(position.x - size.width, position.y - size.height, 0);
            // Bottom 
            hood[6] = new Vector3(position.x, position.y - size.height, 0);
            // Bottom Right
            hood[7] = new Vector3(position.x + size.width, position.y - size.height, 0);

            return hood;
        }
    }
}