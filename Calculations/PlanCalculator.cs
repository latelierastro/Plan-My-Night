﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanMyNight.Calculations {
    /// <summary>
    /// Main computation class for the exposure planning plugin.
    /// </summary>
    public static class PlanCalculator {
        public static ExposureResult Calculate(ExposureRequest request) {
            var result = new ExposureResult();

            // Get selected filters
            var selectedFilters = request.FiltersSelected
                .Where(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();

            // Loss due to meridian flip (already in minutes)
            double flipLoss = request.EnableMeridianFlip ? request.FlipDuration : 0;

            // Compute autofocus loss
            double autofocusLossRGB = 0;
            double autofocusLossSHO = 0;
            if (request.AutofocusFrequency > 0) {
                double numAutofocus = request.TotalAvailableMinutes / request.AutofocusFrequency;

                if (request.EnableAutofocusRGB) {
                    autofocusLossRGB = numAutofocus * (request.AutofocusDurationRGB / 60.0); // sec to min
                }

                if (request.EnableAutofocusSHO && selectedFilters.Any(f => f is "Ha" or "S" or "O")) {
                    autofocusLossSHO = numAutofocus * (request.AutofocusDurationSHO / 60.0); // sec to min
                }
            }

            // Total fixed losses
            double totalLoss = flipLoss + autofocusLossRGB + autofocusLossSHO;

            // Apply safety margin
            double safeTime = request.TotalAvailableMinutes * (1 - request.SafetyTolerance / 100.0);
            double timeAvailableAfterLoss = safeTime - totalLoss;

            // Compute total weight for distribution
            double totalWeight = selectedFilters.Sum(f => request.TargetProportion.GetValueOrDefault(f, 0));
            if (totalWeight <= 0) totalWeight = 1;

            foreach (var filter in selectedFilters) {
                double weight = request.TargetProportion.GetValueOrDefault(filter, 0);
                double timeForFilter = timeAvailableAfterLoss * (weight / totalWeight);

                // Subtract already acquired time
                double alreadyAcquired = request.AlreadyAcquiredPerFilter.GetValueOrDefault(filter, 0);
                double timeRemaining = Math.Max(0, timeForFilter - alreadyAcquired);

                double exposureSec = request.ExposurePerFilter.GetValueOrDefault(filter, 1);
                double pauseSec = request.PauseBetweenExposures;
                double timePerFrameMin = (exposureSec + pauseSec) / 60.0;

                if (timePerFrameMin <= 0) {
                    result.FramesToAcquire[filter] = 0;
                    result.TimePlannedPerFilter[filter] = alreadyAcquired; // <- NE PAS OUBLIER ce temps déjà acquis
                    continue;
                }

                int estimatedFrames = (int)Math.Floor(timeRemaining / timePerFrameMin);

                double ditherLoss = 0;
                int finalFrames = estimatedFrames;
                if (request.EnableDithering && request.DitheringFrequency > 0 && request.DitheringDuration > 0) {
                    int numDithers = estimatedFrames / (int)request.DitheringFrequency;
                    ditherLoss = numDithers * (request.DitheringDuration / 60.0);
                    double timeLeft = timeRemaining - ditherLoss;
                    finalFrames = (int)Math.Floor(timeLeft / timePerFrameMin);
                }

                // TOTAL pour CE FILTRE = temps déjà acquis + nouveaux frames + pertes par dithering
                double totalPlanned = alreadyAcquired + finalFrames * timePerFrameMin + ditherLoss;

                result.FramesToAcquire[filter] = finalFrames;
                result.TimePlannedPerFilter[filter] = totalPlanned;
            }



            // Final result summary
            double totalAlreadyAcquired = selectedFilters.Sum(f => request.AlreadyAcquiredPerFilter.GetValueOrDefault(f, 0));
            result.TotalUsedMinutes = result.TimePlannedPerFilter.Values.Sum() + totalAlreadyAcquired;
            result.UnusedMinutes = timeAvailableAfterLoss - result.TotalUsedMinutes + totalAlreadyAcquired;
            result.Comment = $"Time usage: {Math.Round(result.TotalUsedMinutes, 1)} / {Math.Round(request.TotalAvailableMinutes, 1)} min";

            return result;
        }
    }
}
