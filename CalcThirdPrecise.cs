using System;
using System.Collections.Generic;
using System.Text;

namespace ScubaLib {
  public class CalcThirdPrecise : CalcThirdCommand {

    public CalcThirdPrecise(double volume, int pressure, float baseline, int tankCount)
      : base(volume, pressure, baseline, tankCount) {
    }

    public override void Execute() {
      this.TurnPressure = (int)(pressure - (volume / ((baseline / 100) * tankCount)));
    }
  }
}
