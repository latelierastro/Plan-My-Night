using System;
using System.Collections.Generic;

namespace PlanMyNight.Calculations {
    
    /// User Enries to get for calculation
    
    public class ExposureRequest {
        
        /// Get & set Exp time , already done exposure and percent for each filter 
                
        /// L Filter
        public double L_exp { get; set; }
        public double L_h_done { get; set; }
        public double L_m_done { get; set; }
        public double L_percent { get; set; }

        /// R Filter
        public double R_exp { get; set; }
        public double R_h_done { get; set; }
        public double R_m_done { get; set; }
        public double R_percent { get; set; }

        /// G Filter
        public double G_exp { get; set; }
        public double G_h_done { get; set; }
        public double G_m_done { get; set; }
        public double G_percent { get; set; }

        /// B Filter
        public double B_exp { get; set; }
        public double B_h_done { get; set; }
        public double B_m_done { get; set; }
        public double B_percent { get; set; }

        /// H Filter
        public double H_exp { get; set; }
        public double H_h_done { get; set; }
        public double H_m_done { get; set; }
        public double H_percent { get; set; }

        /// S Filter
        public double S_exp { get; set; }
        public double S_h_done { get; set; }
        public double S_m_done { get; set; }
        public double S_percent { get; set; }

        /// O Filter
        public double O_exp { get; set; }
        public double O_h_done { get; set; }
        public double O_m_done { get; set; }
        public double O_percent { get; set; }



        public double TotalAvailableMinutes { get; set; }

        public double AutofocusDurationRGB { get; set; }
        public double AutofocusDurationSHO { get; set; }
        public double AutofocusFrequency { get; set; }

        public double FlipDuration { get; set; }
        public double DitheringDuration { get; set; }
        public double DitheringFrequency { get; set; }
        public double PauseBetweenExposures { get; set; }

        public double SafetyTolerance { get; set; }

        public Dictionary<string, bool> FiltersSelected { get; set; } = new();
        public Dictionary<string, double> ExposurePerFilter { get; set; } = new();
        public Dictionary<string, double> AlreadyAcquiredPerFilter { get; set; } = new();
        public Dictionary<string, double> TargetProportion { get; set; } = new();
    }
}
