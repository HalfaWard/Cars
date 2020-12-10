namespace Engine.Overlay
{
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="OverlayManager" />
    /// </summary>
    public class OverlayManager
    {
        /// <summary>
        /// Defines the OverlayType
        /// </summary>
        public enum OverlayType
        { /// <summary>
          /// Defines the Normal
          /// </summary>
            Normal,
            /// <summary>
            /// Defines the Static
            /// </summary>
            Static
        }

        /// <summary>
        /// Defines the overlaysList
        /// </summary>
        private List<Overlay> overlaysList = new List<Overlay>();

        /// <summary>
        /// Defines the staticOverlaysList
        /// </summary>
        private List<Overlay> staticOverlaysList = new List<Overlay>();

        /// <summary>
        /// Adds an overlay to the manager. Can be static
        /// </summary>
        /// <param name="overlay">The overlay<see cref="Overlay"/></param>
        /// <param name="isStatic">The isStatic<see cref="bool"/></param>
        public void AddOverlay(Overlay overlay, bool isStatic = false)
        {
            if (isStatic)
            {
                staticOverlaysList.Add(overlay);
            }
            else
            {
                overlaysList.Add(overlay);
            }
        }

        /// <summary>
        /// Adds a list of overlays to the manager. Can be static
        /// </summary>
        /// <param name="overlays">The overlays<see cref="List{Overlay}"/></param>
        /// <param name="isStatic">The isStatic<see cref="bool"/></param>
        public void AddOverlay(List<Overlay> overlays, bool isStatic = false)
        {
            if (isStatic)
            {
                staticOverlaysList.AddRange(overlays);
            }
            else
            {
                overlaysList.AddRange(overlays);
            }
        }

        /// <summary>
        /// Gets a non-static overlay
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <returns>The <see cref="Overlay"/></returns>
        public Overlay GetOverlay(string name)
        {
            return overlaysList.Find(o => o.Name.Equals(name));
        }

        /// <summary>
        /// Gets a static overlay
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <returns>The <see cref="Overlay"/></returns>
        public Overlay GetStaticOverlay(string name)
        {
            return staticOverlaysList.Find(o => o.Name.Equals(name));
        }

        /// <summary>
        /// Checks if manager contains the non-static overlay
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool ContainsOverlay(string name)
        {
            return overlaysList.Exists(o => o.Name.Equals(name));
        }

        /// <summary>
        /// Checks if manager contains the static overlay
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool ContainsStaticOverlay(string name)
        {
            return staticOverlaysList.Exists(o => o.Name.Equals(name));
        }

        /// <summary>
        /// Removes the non-static overlay
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        public void RemoveOverlay(string name)
        {
            if (ContainsOverlay(name))
            {
                overlaysList.RemoveAt(overlaysList.FindIndex(o => o.Name.Equals(name)));
            }
        }

        /// <summary>
        /// Removes the static overlay
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        public void RemoveStaticOverlay(string name)
        {
            if (ContainsStaticOverlay(name))
            {
                staticOverlaysList.RemoveAt(staticOverlaysList.FindIndex(o => o.Name.Equals(name)));
            }
        }

        /// <summary>
        /// Actives an overlay
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="type">The type<see cref="OverlayType"/></param>
        public void ActivateOverlay(string name, OverlayType type)
        {
            if (type == OverlayType.Normal)
            {
                overlaysList.Find(o => o.Name.Equals(name)).IsActive = true;
            }
            else if (type == OverlayType.Static)
            {
                staticOverlaysList.Find(o => o.Name.Equals(name)).IsActive = true;
            }
        }

        /// <summary>
        /// Deactvates an overlay
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="type">The type<see cref="OverlayType"/></param>
        public void DeactivateOverlay(string name, OverlayType type)
        {
            if (type == OverlayType.Normal)
            {
                overlaysList.Find(o => o.Name.Equals(name)).IsActive = false;
            }
            else if (type == OverlayType.Static)
            {
                staticOverlaysList.Find(o => o.Name.Equals(name)).IsActive = false;
            }
        }

        /// <summary>
        /// Activates a list of overlays
        /// </summary>
        /// <param name="names">The names<see cref="string[]"/></param>
        /// <param name="type">The type<see cref="OverlayType"/></param>
        public void ActivateOverlays(string[] names, OverlayType type)
        {
            foreach (string name in names)
            {
                if (type == OverlayType.Normal)
                {
                    overlaysList.Find(o => o.Name.Equals(name)).IsActive = true;
                }
                else if (type == OverlayType.Static)
                {
                    staticOverlaysList.Find(o => o.Name.Equals(name)).IsActive = true;
                }
            }
        }

        /// <summary>
        /// Deactivates a list of overlays
        /// </summary>
        /// <param name="names">The names<see cref="string[]"/></param>
        /// <param name="type">The type<see cref="OverlayType"/></param>
        public void DeactivateOverlays(string[] names, OverlayType type)
        {
            foreach (string name in names)
            {
                if (type == OverlayType.Normal)
                {
                    overlaysList.Find(o => o.Name.Equals(name)).IsActive = false;
                }
                else if (type == OverlayType.Static)
                {
                    staticOverlaysList.Find(o => o.Name.Equals(name)).IsActive = false;
                }
            }
        }

        /// <summary>
        /// Draws all active non-static overlays
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch<see cref="SpriteBatch"/></param>
        public void DrawOverlays(SpriteBatch spriteBatch)
        {
            overlaysList.ForEach(o =>
            {
                if (o.IsActive)
                {
                    o.Draw(spriteBatch);
                }
            });
        }

        /// <summary>
        /// Draws all active static overlays
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch<see cref="SpriteBatch"/></param>
        public void DrawStaticOverlays(SpriteBatch spriteBatch)
        {
            staticOverlaysList.ForEach(o =>
            {
                if (o.IsActive)
                {
                    o.Draw(spriteBatch);
                }
            });
        }

        /// <summary>
        /// Clears the manager
        /// </summary>
        public void Clear()
        {
            overlaysList.Clear();
            staticOverlaysList.Clear();
        }
    }
}
