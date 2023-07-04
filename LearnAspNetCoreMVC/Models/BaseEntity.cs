namespace LearnAspNetCoreMVC.Models
{
    public abstract class BaseEntity
    {
        public DateTime? CreateDate { get; set; } = null;
        public DateTime? UpdateDate { get; set; } = null;
        public DateTime? DeleteDate { get; set; } = null;
        public bool IsDelete { get; set; } = false;
    }
}
