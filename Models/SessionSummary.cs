﻿using System.Collections.Generic;

namespace PlanMyNight.Models {
    /// <summary>
    /// CONTIENT LE RÉSUMÉ D'UNE SESSION DE PLANIFICATION APRÈS CALCUL.
    /// PERMET DE FOURNIR UN BILAN GLOBAL POUR AFFICHAGE ET ANALYSE.
    /// </summary>
    public class SessionSummary {
        // DURÉE ATTRIBUÉE À CHAQUE FILTRE (EN MINUTES)
        public Dictionary<string, double> TimePerFilter { get; set; } = new();

        // NOMBRE TOTAL DE DITHERING PRÉVUS PENDANT LA SESSION
        public double TotalDithers { get; set; }

        // NOMBRE D'AUTOFOCUS EFFECTUÉS SUR LES FILTRES L/R/G/B
        public double TotalAutofocusRGB { get; set; }

        // NOMBRE D'AUTOFOCUS EFFECTUÉS SUR LES FILTRES Ha/SII/OIII
        public double TotalAutofocusSHO { get; set; }

        // TEMPS NON UTILISÉ EN FIN DE SESSION (EN MINUTES)
        public double UnusedTime { get; set; }

        // NOMBRE TOTAL D’AUTOFOCUS EFFECTUÉS (TOUS FILTRES CONFONDUS)
        public double TotalAutofocus => TotalAutofocusRGB + TotalAutofocusSHO;
    }
}
