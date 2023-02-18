using Blahem.Application.Common.Interfaces;

namespace Blahem.Infrastructure.DateTimes;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}