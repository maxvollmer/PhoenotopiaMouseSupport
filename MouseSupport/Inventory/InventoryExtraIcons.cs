using UnityEngine;
using System.Collections.Generic;
using MouseSupport.Helpers;
using TMPro;

namespace MouseSupport.Inventory
{
    public class InventoryExtraIcons
    {
        public enum IconID
        {
            SORT,
            TRASHCAN
        }

        private struct Icon
        {
            public SpriteRenderer sprite;
            public Vector2 offset;
        }

        private static Dictionary<IconID, Icon> icons = new Dictionary<IconID, Icon>();

        public static void Update(ItemGridLogic item_grid)
        {
            if (icons.Count == 0)
            {
                InitIcons(item_grid);
            }

            Vector2 iconAnchorPos = ReflectionHelper.GetMemberValue<Component>(item_grid, "_capacity_text").transform.position;

            foreach (var icon in icons.Values)
            {
                icon.sprite.transform.position = new Vector3(
                    iconAnchorPos.x + icon.offset.x,
                    iconAnchorPos.y + icon.offset.y,
                    icon.sprite.transform.position.z);
            }
        }

        public static bool IsMouseOver(IconID iconID, Vector2 cursorPos, SpriteRenderer cursorSprite)
        {
            if (icons.TryGetValue(iconID, out Icon icon))
            {
                return CursorSpriteHelper.IsCursorAtSprite(cursorPos, cursorSprite, icon.sprite);
            }
            return false;
        }

        private static void InitIcons(ItemGridLogic item_grid)
        {
            icons.Add(IconID.SORT, new Icon() { sprite = CreateSprite(item_grid, "sort.png", "SORT"), offset = new Vector2(0f, 5.5f) });
            icons.Add(IconID.TRASHCAN, new Icon() { sprite = CreateSprite(item_grid, "trash.png"), offset = new Vector2(0f, -2.5f) });
        }

        private static SpriteRenderer CreateSprite(ItemGridLogic item_grid, string filename, string subtitle = null)
        {
            var spriteTemplate = ReflectionHelper.GetMemberValue<SpriteRenderer>(item_grid, "_cursor_sprite");

            var spriteObject = new GameObject(filename, typeof(SpriteRenderer));
            spriteObject.transform.SetParent(spriteTemplate.transform.parent, true);
            spriteObject.layer = GL.num_layer_SEPARATE_GUI;

            spriteObject.transform.position = spriteTemplate.transform.position;
            spriteObject.transform.localScale = spriteTemplate.transform.lossyScale * 0.75f;

            var sprite = spriteObject.GetComponent<SpriteRenderer>();
            sprite.sortingLayerID = GL.sort_layer_ui_elements;
            sprite.sortingOrder = 1999;

            var texture = LoadTexture(filename);

            sprite.sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                spriteTemplate.sprite.pixelsPerUnit);

            if (subtitle != null)
            {
                var text = Object.Instantiate(ReflectionHelper.GetMemberValue<TextMeshPro>(item_grid, "_capacity_text"));
                text.transform.SetParent(spriteObject.transform, true);
                text.transform.localPosition = new Vector3(0f, -1f, 0f);
                text.text = subtitle;
            }

            return sprite;
        }

        private static Texture2D LoadTexture(string filename)
        {
            var bytes = System.IO.File.ReadAllBytes(MainEntry.Mod.Path + "Data\\" + filename);
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                anisoLevel = 1
            };
            texture.LoadImage(bytes);
            return texture;
        }
    }
}
