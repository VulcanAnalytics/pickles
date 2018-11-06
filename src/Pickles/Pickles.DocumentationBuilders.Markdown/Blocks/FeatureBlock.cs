﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FeatureBlock.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
//
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

using PicklesDoc.Pickles.ObjectModel;
using System;
using System.Collections.Generic;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.Blocks
{
    class FeatureBlock
    {
        readonly Feature feature;
        readonly Stylist style;

        public FeatureBlock(Feature feature, Stylist style)
        {
            this.feature = feature;
            this.style = style;
        }

        public new string ToString()
        {
            return RenderedBlock();
        }

        private string RenderedBlock()
        {
            var lines = new List<string>();

            lines = AddFeatureTagsIfAvailable(lines);

            lines = AddHeading(lines);

            lines = AddDescriptionIfAvailable(lines);

            lines = AddScenariosIfAvailable(lines);

            return LineCollectionToString(lines);
        }

        private List<string> AddFeatureTagsIfAvailable(List<string> lines)
        {
            if (feature.Tags.Count > 0)
            {
                var tagline = String.Empty;

                foreach (var tag in feature.Tags)
                {
                    tagline = string.Concat(tagline, style.AsTag(tag), " ");
                }

                lines.Add(tagline.TrimEnd());
                lines.Add(string.Empty);
            }
            return lines;
        }

        private List<string> AddHeading(List<string> lines)
        {
            lines.Add(style.AsFeatureHeading(feature.Name));
            lines.Add(string.Empty);

            return lines;
        }

        private List<string> AddDescriptionIfAvailable(List<string> lines)
        {
            if (feature.Description != null)
            {
                foreach (var descriptionLine in feature.Description.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    lines.Add(descriptionLine);
                    lines.Add(string.Empty);
                }
            }

            return lines;
        }

        private List<string> AddScenariosIfAvailable(List<string> lines)
        {
            if (feature.FeatureElements.Count > 0)
            {
                foreach (var element in feature.FeatureElements)
                {
                    lines = AddScenario(lines, element as Scenario);
                }
            }

            return lines;
        }

        private List<string> AddScenario(List<string> lines, Scenario scenario)
        {
            var scenarioBlock = new ScenarioBlock(scenario, style);

            lines.Add(scenarioBlock.ToString());

            return lines;
        }

        private string LineCollectionToString(List<string> lines)
        {
            string result = string.Empty;

            foreach (var line in lines)
            {
                result = string.Concat(result, line, Environment.NewLine);
            }

            return result;
        }
    }
}