namespace TelgeProject.Entity
{
    public class tblKraDescription
    {
        public int Id { get; set; }
        public int? KraIdFk { get; set; }
        public string? Description { get; set; }

        public virtual TblKra? KraIdFkNavigation { get; set; }
    }
}