using System;
using System.Runtime.InteropServices;
using System.Threading;
using OposScanner_CCO;
using OposScale_CCO;
using OPOSCONSTANTSLib;

namespace Zebra_MP7000_OPOS
{
    class Program
    {
        private static OPOSScanner scanner;
        private static OPOSScale scale;
        private static bool scaleEnabled = false;

        static void Main(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                exitEvent.Set();
            };

            try
            {
                // Initialize Scanner
                if (InitializeScanner())
                {
                    Console.WriteLine("Scanner initialized successfully");
                }
                else
                {
                    Console.WriteLine("Scanner initialization failed");
                }

                // Initialize Scale
                if (InitializeScale())
                {
                    Console.WriteLine("Scale initialized successfully");
                    scaleEnabled = true;
                }
                else
                {
                    Console.WriteLine("Scale initialization failed");
                }

                if (scanner?.Claimed == true || scale?.Claimed == true)
                {
                    Console.WriteLine("\nDevice(s) ready. Scanner: scan barcodes | Scale: live weight monitoring");
                    Console.WriteLine("Press Ctrl+C to exit");

                    // Wait for exit event
                    exitEvent.WaitOne();

                    // Cleanup
                    CleanupDevices();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                CleanupDevices();
            }
        }

        static bool InitializeScanner()
        {
            try
            {
                Type scannerType = Type.GetTypeFromProgID("OPOS.Scanner");
                scanner = (OPOSScanner)Activator.CreateInstance(scannerType);

                int result = scanner.Open("ZEBRA_SCANNER");
                if (result != 0)
                {
                    Console.WriteLine($"Failed to open scanner: {result}");
                    return false;
                }

                result = scanner.ClaimDevice(1000);
                if (result != 0)
                {
                    Console.WriteLine($"Failed to claim scanner: {result}");
                    return false;
                }

                if (scanner.Claimed)
                {
                    scanner.DataEvent += ScannerDataEvent;
                    scanner.DeviceEnabled = true;
                    scanner.DataEventEnabled = true;
                    scanner.DecodeData = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Scanner initialization error: {ex.Message}");
            }
            return false;
        }

        static bool InitializeScale()
        {
            try
            {
                Type scaleType = Type.GetTypeFromProgID("OPOS.Scale");
                scale = (OPOSScale)Activator.CreateInstance(scaleType);

                int result = scale.Open("ZEBRA_SCALE");
                if (result != 0)
                {
                    Console.WriteLine($"Failed to open scale: {result}");
                    return false;
                }

                result = scale.ClaimDevice(1000);
                if (result != 0)
                {
                    Console.WriteLine($"Failed to claim scale: {result}");
                    return false;
                }

                if (scale.Claimed)
                {
                    // Enable live weighing
                    scale.StatusNotify = (int)OPOSScaleConstants.SCAL_SN_ENABLED;

                    if (scale.ResultCode == (int)OPOS_Constants.OPOS_SUCCESS) // OPOS_SUCCESS
                    {
                        scale.StatusUpdateEvent += ScaleStatusUpdateEvent;
                        scale.DeviceEnabled = true;
                        scale.DataEventEnabled = true;

                        Console.WriteLine($"Scale max weight: {scale.MaximumWeight / 1000.0:F3} lbs");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Scale initialization error: {ex.Message}");
            }
            return false;
        }

        static void ScannerDataEvent(int value)
        {
            try
            {
                Console.WriteLine($"\n[SCAN] {scanner.ScanDataLabel}");
                scanner.DataEventEnabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Scanner event error: {ex.Message}");
            }
        }

        static private void ScaleStatusUpdateEvent(int value)
        {
            int status = (int)scale.ResultCode;

            if (value == (int)OPOSScaleConstants.SCAL_SUE_STABLE_WEIGHT)
            {
                Console.WriteLine(WeightFormat(scale.ScaleLiveWeight));
            }
            else if (value == (int)OPOSScaleConstants.SCAL_SUE_WEIGHT_UNSTABLE)
            {
                Console.WriteLine("Scale weight unstable");
            }
            else if (value == (int)OPOSScaleConstants.SCAL_SUE_WEIGHT_ZERO)
            {
                Console.WriteLine(WeightFormat(scale.ScaleLiveWeight));
            }
            else if (value == (int)OPOSScaleConstants.SCAL_SUE_WEIGHT_OVERWEIGHT)
            {
                Console.WriteLine("Weight limit exceeded.");
            }
            else if (value == (int)OPOSScaleConstants.SCAL_SUE_NOT_READY)
            {
                Console.WriteLine("Scale not ready.");
            }
            else if (value == (int)OPOSScaleConstants.SCAL_SUE_WEIGHT_UNDER_ZERO)
            {
                Console.WriteLine("Scale under zero weight.");
            }
            else
            {
                Console.WriteLine("Unknown status [{0}]", value);
            }
        }

        static private string WeightFormat(int weight)
        {
            string weightStr = string.Empty;

            string units = UnitAbbreviation(scale.WeightUnits);
            if (units == string.Empty)
            {
                weightStr = string.Format("Unknown weight unit");
            }
            else
            {
                double dWeight = 0.001 * (double)weight;
                weightStr = string.Format("{0:0.000} {1}", dWeight, units);
            }

            return weightStr;
        }

        static private string UnitAbbreviation(int units)
        {
            string unitStr = string.Empty;

            switch ((OPOSScaleConstants)units)
            {
                case OPOSScaleConstants.SCAL_WU_GRAM: unitStr = "gr."; break;
                case OPOSScaleConstants.SCAL_WU_KILOGRAM: unitStr = "kg."; break;
                case OPOSScaleConstants.SCAL_WU_OUNCE: unitStr = "oz."; break;
                case OPOSScaleConstants.SCAL_WU_POUND: unitStr = "lb."; break;
            }

            return unitStr;
        }

        static void CleanupDevices()
        {
            try
            {
                if (scanner?.Claimed == true)
                {
                    scanner.DataEvent -= ScannerDataEvent;
                    scanner.DataEventEnabled = false;
                    scanner.DeviceEnabled = false;
                    scanner.ReleaseDevice();
                    scanner.Close();
                    Console.WriteLine("Scanner closed");
                }

                if (scale?.Claimed == true)
                {
                    scale.StatusUpdateEvent -= ScaleStatusUpdateEvent;
                    scale.DataEventEnabled = false;
                    scale.DeviceEnabled = false;
                    scale.ReleaseDevice();
                    scale.Close();
                    Console.WriteLine("Scale closed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cleanup error: {ex.Message}");
            }
        }
    }
}