/// <summary>Utility math functions</summary>
/// <Author>Larry Hack</Author> 
/// <Created>2005</Created>
/// <Comments>Many of these calculations are based on the "Pressure Formula" 
///     Pg
/// ----------
///  Fg | P
/// Where 
/// Pg = Partial Pressure of a gas
/// Fg = Fraction of gas (decimal equivalent of the % of gas
/// P  = Presssure Total or ATA (ATmosphere Absolute)
/// Depth = (P -1) * 33|34  33 for saltwater, 34 for freshwater
/// </Comments>
/// <Modified>
///   <ModifiedBy>Larry Hack</ModifiedBy>
///   <DateTime>06/23/2007 7:06PM</DateTime>
///   <Description>Added MaxOperatingDepth and EquivalentAirDepth</Description>
/// </Modified>
/// <Modified>
///   <ModifiedBy>Larry Hack</ModifiedBy>
///   <DateTime>07/03/2007 12:25AM</DateTime>
///   <Description>Added EquivalentNitrogenDepth</Description>
/// </Modified>
/// <Modified>
///   <ModifiedBy>Larry Hack</ModifiedBy>
///   <DateTime>07/04/2007 6:02PM</DateTime>
///   <Description>Added detailed comments for class, changed water params to depthPerATA, 
///                added AtaToDepth & DepthToAta, changed MaxOperatingDepth to use AtaToDepth</Description>
/// </Modified> 
using System;
using System.Collections.Generic;
using System.Text;

namespace ScubaLib {
  public class Math {
    //feet per ATA
    public static int FFW = 34;
    public static int FSW = 33;
    public static int MSW = 10;
    public static int MFW = 0; //??

    /// <summary>Convert ATmosphere Absolute to depth</summary>
    /// <param name="ata"></param>
    /// <param name="depthPerATA">FFW|FSW|MSW|MFW|other? Red Sea for instance</param>
    public static int AtaToDepth(double ata, int depthPerATA) {
      double depth;
      depth = (ata - 1) * depthPerATA;
      //return (int)System.Math.Round(depth, 1);
      return (int)depth;
    }

    /// <summary>Calculate best oxygen gas mix for planned depth</summary>
    /// <remarks>Pressure Formula Fg = Pg/P
    /// Best O2 mix in order not to exceed CNS O2 toxicity clock</remarks>
    /// <param name="depth"></param>
    /// <param name="ppO2"></param>
    /// <param name="depthPerATA"></param>
    public static double BestO2Mix(int depth, double ppO2, int depthPerATA) {
      int intO2;
      double o2 = ppO2 / DepthToAta(depth, depthPerATA);
      //Round it down to the lower % for conservancy
      intO2 = (int)(o2 * 100);
      o2 = intO2 / 100.0;
      return o2;
    }
    
    /// <summary>Convert depth to ATmosphere Absolute</summary>
    /// <param name="depth"></param>
    /// <param name="depthPerATA">FFW|FSW|MSW|MFW|other? Red Sea for instance</param>
    public static double DepthToAta(int depth, int depthPerATA){
      double ata;
      ata = ((double)depth / depthPerATA) + 1;
      return System.Math.Round(ata, 2);      
    }

    /// <summary>Calculate the equivalent air depth of an enriched o2 mix</summary>
    /// <remarks>.79 = inert gas content of air</remarks>
    /// <param name="o2"></param><param name="depth"></param><param name="depthPerATA">FFW|FSW|MSW|MFW|other?</param>
    /// <returns>int</returns>
    public static int EquivalentAirDepth(int o2, int depth, int depthPerATA) {
      int retVal = 0;
      retVal = (int)System.Math.Round((((1.0 - (o2 / 100.0)) * (depth + depthPerATA)) / .79) - depthPerATA, 0);
      return retVal;
    }

    /// <summary>Calculate the equivalent nitrogen depth of a mix that includes helium</summary>
    /// <remarks></remarks>
    /// <param name="helium"></param><param name="depth"></param><param name="depthPerATA"</param>
    /// <returns>int</returns>
    public static int EquivalentNitrogenDepth(int helium, int depth, int depthPerATA) {
      int retVal;
      double val = (1 - (helium / 100.0)) * (depth + depthPerATA) - depthPerATA;
      retVal = (int)System.Math.Round(val,0);
      return retVal;
    }

    /// <summary>Returns Maximum Operating Depth based on oxygen in mix</summary>
    /// <Remarks>Pressure Formula P = Pg/Fg</Remarks>
    /// <param name="ppo2">Partial Pressure of Oxygen</param>
    /// <param name="o2">Fraction of Oxygen</param>
    /// <param name="depthPerATA">33 for freshwater, 34 for saltwater</param>
    /// <returns>int</returns>
    public static int MaxOperatingDepth(double ppo2, int o2, int depthPerATA) {
      int retVal;
      double val;
      //double val2;
      val = AtaToDepth(ppo2 / (o2 / 100.0), depthPerATA);
      //val2 = ((ppo2 / (o2 / 100.0)) - 1) * depthPerATA;
      retVal = (int)val;
      return retVal;
    }

    /// <summary>Returns Maximum Operating Depth based on oxygen and helium in mix</summary>
    /// <param name="O2"></param>
    /// <returns>int</returns>
    public static int MaxOperatingDepth(int O2, int he) {
      return 0;
    }

    /// <summary>Round pressure down to calculate 3rds, e.g. 3469 to 3300</summary>
    public static int RoundPressure(int pressure) {
      int remainder;
      int adjuster = 0;
      int roundedPressure = 0;

      //round it down to the hundreths position
      roundedPressure = HackLib.System.Math.RoundDown(pressure, 100);

      //see if it's evenly divisible by 3
      remainder = roundedPressure % 3;
      //adjust it if necessary
      adjuster = remainder * 100;
      roundedPressure -= adjuster;
      return roundedPressure;
    }
  }
}
