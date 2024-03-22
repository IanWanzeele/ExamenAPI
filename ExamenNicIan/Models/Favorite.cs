namespace ExamenNicIan.Models
{
    public class Favorite
    {
        public int FavoriteID { get; set; }
        public int UserID { get; set; }
        public long RestaurantID { get; set; }
        public string RestaurantName { get; set; }
        public User User { get; set; }
        
    }
}
