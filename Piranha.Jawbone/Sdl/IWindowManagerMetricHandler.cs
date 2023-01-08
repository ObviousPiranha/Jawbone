using System;

namespace Piranha.Jawbone.Sdl;

public interface IWindowManagerMetricHandler
{
    void ReportSleepTime(TimeSpan sleepTime);
}
