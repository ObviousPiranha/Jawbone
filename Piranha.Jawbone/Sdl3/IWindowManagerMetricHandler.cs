using System;

namespace Piranha.Jawbone.Sdl3;

public interface IWindowManagerMetricHandler
{
    void ReportSleepTime(TimeSpan sleepTime);
}
