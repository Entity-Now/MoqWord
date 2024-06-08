using FreeScheduler;

namespace MoqWord.Services
{
    public static class SchedulerService
    {
        public static Scheduler getScheduler()
        {
            return new FreeSchedulerBuilder()
                .Build();
        }
    }
}
