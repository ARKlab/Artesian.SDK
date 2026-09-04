using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Email notification configuration for quality alerts.
    /// Each entry in the <see cref="Recipients"/> array specifies a recipient email address that will receive the alert notification.
    /// </summary>
    [MessagePackObject]
    public class MailNotificationDto
    {
        /// <summary>
        /// The array of recipient email addresses to which the quality alert notification will be sent.
        /// </summary>
        [Key(0)]
        public string[] Recipients { get; set; } = System.Array.Empty<string>();
    }
}
