using System;

namespace Cheeseria.Data.Models
{
    /// <summary>
    /// Cheese Model
    /// </summary>
    [Serializable]
    public class Cheese
    {
        /// <summary>
        /// Cheese Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of Cheese
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Descriptive name of cheese colour.
        /// </summary>
        public string Colour { get; set; }

        /// <summary>
        /// Image data (Base64 encoded when sent via web API).
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Cost of cheese in cents per KG.
        /// </summary>
        public int CostInCentsPerKg { get; set; }

        /// <summary>
        /// Flag to indicate if cheese should be displayed to the general public on main page.
        /// </summary>
        public bool Published { get; set; }

    }
}
