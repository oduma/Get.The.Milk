﻿using System;
using System.Collections.Generic;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.Interactions;
using Newtonsoft.Json;

namespace GetTheMilk.BaseCommon
{
    public interface IActionEnabled
    {

        PerformActionResult PerformAction(BaseActionTemplate actionTemplate);

        T CreateNewInstanceOfAction<T>(string uniqueId) where T : BaseActionTemplate;

        BaseActionTemplate CreateNewInstanceOfAction(string uniqueId);

        [JsonIgnore]
        Dictionary<string,BaseActionTemplate> AllActions { get;}

        [JsonIgnore]
        SortedList<string, Interaction[]> Interactions { get; set; }

        bool CanPerformAction(BaseActionTemplate actionTemplate);

        void AddAvailableAction(BaseActionTemplate baseActionTemplate);
    }
}
