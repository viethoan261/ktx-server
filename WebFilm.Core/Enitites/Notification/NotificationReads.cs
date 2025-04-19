using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm.Core.Enitites.Notification
{
    public class NotificationReads : BaseEntity
    {
        #region Prop
        public int user_id { get; set; }
        
        public int notification_id { get; set; }
        
        public DateTime? read_at { get; set; }
        #endregion
    }
} 