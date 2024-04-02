using System;

namespace Piranha.Jawbone.Sdl2;

public interface IWindowManagerMetricHandler
{
    void ReportSleepTime(TimeSpan sleepTime);
}
