using System;
using System.Collections.Generic;
using System.Text;

namespace ScubaLib {
  public abstract class CalcThirdCommand : ICommand {
    public int TurnPressure;

    protected double volume;
    protected int pressure;
    protected float baseline;
    protected int tankCount;

    public CalcThirdCommand(double volume, int pressure, float baseline, int tankCount) {
      this.volume = volume;
      this.pressure = pressure;
      this.baseline = baseline;
      this.tankCount = tankCount;
    }

    public abstract void Execute();
  }
}
