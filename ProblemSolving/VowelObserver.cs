﻿using System.Collections.Generic;
using CrowdCode.Library.Modules.Language.ProblemSolving;
using Starship.Core.ProblemSolving;

namespace Starship.Language.ProblemSolving {

    public class VowelObserver : PatternObserver<SyllableResult> {
        public override void GetObservations(SyllableResult fact, List<Observation> observaitons) {
            foreach (var syllable in fact.Syllables) {
                
            }
        }
    }
}