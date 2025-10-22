using System.ComponentModel.DataAnnotations;

namespace gozba_na_klik.Dtos.Users
{
    public class DaySlotDto
    {
        [Range(0, 6)]
        public int DayOfWeek { get; set; }
        [RegularExpression(@"^\d{2}:\d{2}$")]
        public string Start { get; set; } = default!;
        [RegularExpression(@"^\d{2}:\d{2}$")]
        public string End { get; set; } = default!;
    }
    public class WeeklyScheduleUpsertRequestDto
    {
        [MinLength(7), MaxLength(7)]
        public List<DaySlotDto> Days { get; set; } = new();
    }
    public class WeeklyScheduleResponseDto
    {
        public int CourierUserId { get; set; }
        public List<DaySlotDto> Days { get; set; } = new();
        public double WeeklyHours { get; set; }
    }
    public class  CourierStatusResponseDto
    {
        public string Status { get; set; } = "Inactive"; // Active, Inactive i Suspended
        public DateTime CheckedAtLocal { get; set; }
        public string TimeZone { get; set; } = "Europe/Belgrade";
    }
}
