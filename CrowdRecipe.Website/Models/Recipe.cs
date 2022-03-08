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

        [BsonIgnore]
        [Display(Name = "Rating")]
        public int RatingInput { get; set; }

        [BsonIgnore]
        public float Rating
        {
            get
            {
                if(Ratings.Count > 0)
                {
                    float rating = 0;
                    foreach (float r in Ratings.Values)
                    {
                        rating += r;
                    }
                    rating /= Ratings.Count;
                    return rating;
                }
                return 0;
            }
            private set { }
        }

        [BsonElement("ratings")]
        public Dictionary<string, float> Ratings { get; set; }



        public Recipe()
        {
            Ratings = new Dictionary<string, float>();
        }

    }
}
