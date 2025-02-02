using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Models.Entity

{
    [Table("Goods")]
    public class Goods
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Reserve { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Store_id")]
        public string? StoreId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("Img_path")]
        [Display(Name = "Image")]
        public string? ImgPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? State { get; set; }
    }
}
