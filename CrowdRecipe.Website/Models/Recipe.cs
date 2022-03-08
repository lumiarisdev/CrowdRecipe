using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdRecipe.Website.Models
{

    [BsonIgnoreExtraElements]
    public class Recipe
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("author_id")]
        public int AuthorId { get; set; }
        [BsonElement("author_name")]
        [Display(Name = "Author")]
        public string AuthorName { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("prose")]
        public string Prose { get; set; }
        [BsonElement("prep_time_minutes")]
        [Display(Name = "Prep Time")]
        public int PrepTimeMinutes { get; set; }
        [BsonElement("cook_time_minutes")]
        [Display(Name = "Cook Time")]
        public int CookTimeMinutes { get; set; }
        [BsonElement("ingredients")]
        public string Ingredients { get; set; }
        [BsonElement("instructions")]
        public string Instructions { get; set; }

        public Recipe()
        {
            
        }

    }
}
